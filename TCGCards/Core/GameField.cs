using System;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core.SpecialAbilities;

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
            AttackStoppers = new List<AttackStopper>();
            DamageStoppers = new List<DamageStopper>();
        }

        public void Init()
        {
            Players.Add(new Player());
            Players.Add(new Player());
            ActivePlayer = Players[new Random().Next(Players.Count)];
        }

        public void RevealCardsTo(List<Card> pickedCards, Player nonActivePlayer)
        {
            //TODO: Complete this
        }

        public void InitTest()
        {
            Players.Add(new Player { Id = Guid.NewGuid() });
            Players.Add(new Player { Id = Guid.NewGuid() });
            ActivePlayer = Players[0];
        }

        public void OnActivePokemonSelected(Player owner, PokemonCard activePokemon)
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

        public void OnBenchPokemonSelected(Player owner, PokemonCard selectedPokemon)
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
                ActivePlayer.ActivePokemonCard.Ability?.Activate(ActivePlayer, NonActivePlayer, 0);

            if (!AttackStoppers.Any(x => x.IsAttackIgnored()) && !ActivePlayer.ActivePokemonCard.AttackStoppers.Any(x => x.IsAttackIgnored()))
            {
                if (!DamageStoppers.Any(x => x.IsDamageIgnored()))
                {
                    var damage = attack.GetDamage(ActivePlayer, NonActivePlayer);
                    damage.NormalDamage = GetDamageAfterWeaknessAndResistance(damage.NormalDamage, ActivePlayer.ActivePokemonCard, NonActivePlayer.ActivePokemonCard);

                    NonActivePlayer.ActivePokemonCard.DealDamage(damage);

                    if (NonActivePlayer.ActivePokemonCard.Ability?.TriggerType == TriggerType.TakesDamage)
                        NonActivePlayer.ActivePokemonCard.Ability?.Activate(ActivePlayer, NonActivePlayer, damage.NormalDamage + damage.DamageWithoutResistAndWeakness);
                }

                attack.ProcessEffects(this, ActivePlayer, NonActivePlayer);
            }

            PostAttack();
        }

        private int GetDamageAfterWeaknessAndResistance(int damage, PokemonCard attacker, PokemonCard defender)
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
            AttackStoppers.ForEach(attackStopper => attackStopper.TurnsLeft--);
            DamageStoppers.ForEach(damageStopper => damageStopper.TurnsLeft--);

            AttackStoppers = AttackStoppers.Where(attackStopper => attackStopper.TurnsLeft > 0).ToList();
            DamageStoppers = DamageStoppers.Where(damageStopper => damageStopper.TurnsLeft > 0).ToList();

            CheckDeadPokemon();
            CheckEndTurn();
        }

        public void PlayPokemon(PokemonCard pokemon)
        {
            ActivePlayer.PlayCard(pokemon);

            if(pokemon.Ability?.TriggerType == TriggerType.EnterPlay)
                pokemon.Ability?.Activate(ActivePlayer, NonActivePlayer, 0);
        }

        private void CheckDeadPokemon()
        {
            if(NonActivePlayer.ActivePokemonCard.IsDead())
            {
                NonActivePlayer.ActivePokemonCard.KnockedOutBy = ActivePlayer.ActivePokemonCard;

                if(NonActivePlayer.ActivePokemonCard.Ability?.TriggerType == TriggerType.Dies)
                    NonActivePlayer.ActivePokemonCard.Ability?.Activate(NonActivePlayer, ActivePlayer, 0);
                if(ActivePlayer.ActivePokemonCard.Ability?.TriggerType == TriggerType.Kills)
                    ActivePlayer.ActivePokemonCard.Ability?.Activate(ActivePlayer, NonActivePlayer, 0);

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

        public void SelectPrizeCard(Card prizeCard)
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

        public List<AttackStopper> AttackStoppers { get; set; }
        public List<DamageStopper> DamageStoppers { get; set; }
    }
}
