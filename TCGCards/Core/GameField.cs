using System;
using System.Collections.Generic;
using System.Linq;

namespace TCGCards.Core
{
    public class GameField
    {
        private const int StartingHandsize = 7;
        private const int PriceCards = 1;
        private object lockObject = new object();
        private HashSet<Guid> playersSetStartBench;

        public GameField()
        {
            Players = new List<Player>();
            StateQueue = new Queue<GameFieldState>();
            playersSetStartBench = new HashSet<Guid>();
        }

        public void Init()
        {
            Players.Add(new Player());
            Players.Add(new Player());
            ActivePlayer = Players[new Random().Next(Players.Count)];
        }

        public void RevealCardsTo(List<ICard> pickedCards, Player nonActivePlayer)
        {
            //TODO: Complete this
        }

        public void InitTest()
        {
            Players.Add(new Player { Id = Guid.NewGuid() });
            Players.Add(new Player { Id = Guid.NewGuid() });
            ActivePlayer = Players[0];
        }

        public void OnActivePokemonSelected(Player owner, IPokemonCard activePokemon)
        {
            owner.SetActivePokemon(activePokemon);

            lock (lockObject)
            {
                if (GameState == GameFieldState.BothSelectingActive)
                {
                    if (Players.All(p => p.ActivePokemonCard != null))
                        GotoNextState();
                }
                else
                {
                    GotoNextState();
                }
            }
        }

        public void OnBenchPokemonSelected(Player owner, IPokemonCard selectedPokemon)
        {
            owner.SetBenchedPokemon(selectedPokemon);

            if (GameState == GameFieldState.BothSelectingBench)
            {
                lock (lockObject)
                {
                    playersSetStartBench.Add(owner.Id);
                    if (playersSetStartBench.Count == 2)
                    {
                        Players.ForEach(x => x.SetPrizeCards(PriceCards));
                        GotoNextState();
                    }
                }
            }
            else
            {
                GotoNextState();
            }
        }

        private void GotoNextState()
        {
            if(StateQueue.Any())
                GameState = StateQueue.Dequeue();

            if(GameState == GameFieldState.EndAttack)
                PostAttack();
        }

        public void StartGame()
        {
            ActivePlayer = Players[new Random().Next(2)];

            foreach(var player in Players)
            {
                player.Deck.Shuffle();
                player.DrawCards(StartingHandsize);
            }

            GameState = GameFieldState.BothSelectingActive;
            StateQueue.Enqueue(GameFieldState.BothSelectingBench);
            StateQueue.Enqueue(GameFieldState.InTurn);
        }

        public void Attack(Attack attack)
        {
            if (StateQueue.Any())
                StateQueue.Dequeue();

            StateQueue.Enqueue(GameFieldState.EndAttack);

            if (ActivePlayer.ActivePokemonCard.Ability?.TriggerType == TriggerType.Attacks)
                ActivePlayer.ActivePokemonCard.Ability?.Activate(ActivePlayer, NonActivePlayer);         

            var damage = attack.GetDamage(ActivePlayer, NonActivePlayer);
            NonActivePlayer.ActivePokemonCard.DamageCounters += damage.DamageWithoutResistAndWeakness;
            NonActivePlayer.ActivePokemonCard.DamageCounters += GetDamageAfterWeaknessAndResistance(damage.NormalDamage, ActivePlayer.ActivePokemonCard, NonActivePlayer.ActivePokemonCard);

            attack.ProcessEffects(this, ActivePlayer, NonActivePlayer);

            PostAttack();
        }

        private int GetDamageAfterWeaknessAndResistance(int damage, IPokemonCard attacker, IPokemonCard defender)
        {
            int realDamage = damage;

            if (defender.Resistance == attacker.PokemonType)
            {
                realDamage -= 30;
            }
            if (defender.Weakness == attacker.PokemonType)
            {
                realDamage *= 2;
            }

            return Math.Max(realDamage, 0);
        }

        public void PostAttack()
        {
            CheckDeadPokemon();
            CheckEndTurn();
        }

        public void PlayPokemon(IPokemonCard pokemon)
        {
            ActivePlayer.PlayCard(pokemon);

            if(pokemon.Ability?.TriggerType == TriggerType.EnterPlay)
                pokemon.Ability?.Activate(ActivePlayer, NonActivePlayer);
        }

        private void CheckDeadPokemon()
        {
            if(NonActivePlayer.ActivePokemonCard.IsDead())
            {
                NonActivePlayer.ActivePokemonCard.KnockedOutBy = ActivePlayer.ActivePokemonCard;

                if(NonActivePlayer.ActivePokemonCard.Ability?.TriggerType == TriggerType.Dies)
                    NonActivePlayer.ActivePokemonCard.Ability?.Activate(NonActivePlayer, ActivePlayer);
                if(ActivePlayer.ActivePokemonCard.Ability?.TriggerType == TriggerType.Kills)
                    ActivePlayer.ActivePokemonCard.Ability?.Activate(ActivePlayer, NonActivePlayer);

                NonActivePlayer.ActivePokemonCard.KnockedOutBy = ActivePlayer.ActivePokemonCard;
                StateQueue.Enqueue(GameFieldState.ActivePlayerSelectingPrize);
                StateQueue.Enqueue(GameFieldState.UnActivePlayerSelectingFromBench);
            }
            if(ActivePlayer.ActivePokemonCard.IsDead())
            {
                ActivePlayer.ActivePokemonCard.KnockedOutBy = NonActivePlayer.ActivePokemonCard;
                StateQueue.Enqueue(GameFieldState.UnActivePlayerSelectingPrize);
                StateQueue.Enqueue(GameFieldState.ActivePlayerSelectingFromBench);
            }
        }

        private void CheckEndTurn()
        {
            if(StateQueue.Any())
                GameState = StateQueue.Dequeue();
            else
                EndTurn();

            if (GameState == GameFieldState.EndAttack)
                EndTurn();
        }

        public void SelectPrizeCard(ICard prizeCard)
        {
            if(GameState == GameFieldState.ActivePlayerSelectingPrize)
                ActivePlayer.DrawPrizeCard(prizeCard);
            else if(GameState == GameFieldState.UnActivePlayerSelectingPrize)
                NonActivePlayer.DrawPrizeCard(prizeCard);

            CheckEndTurn();
        }

        public void EndTurn()
        {
            ActivePlayer.EndTurn();
            CheckDeadPokemon();

            if(StateQueue.Any())
                CheckEndTurn();
            else
            {
                SwapActivePlayer();
                StartNextTurn();
            }
        }

        private void StartNextTurn()
        {
            ActivePlayer.ResetTurn();
            NonActivePlayer.ResetTurn();
            StateQueue.Clear();

            GameState = GameFieldState.TurnStarting;
            ActivePlayer.DrawCards(1);

            GameState = GameFieldState.InTurn;
        }

        public void SwapActivePlayer()
        {
            ActivePlayer = Players.First(x => !x.Id.Equals(ActivePlayer.Id));
        }

        public GameFieldState GameState { get; set; }

        public List<Player> Players { get; set; }
        public Player ActivePlayer { get; set; }
        public Player NonActivePlayer { get { return Players.First(p => p.Id != ActivePlayer.Id); } }
        public int Mode { get; set; }
        public Queue<GameFieldState> StateQueue { get; set; }
    }
}
