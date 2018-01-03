using System;
using System.Collections.Generic;
using System.Linq;

namespace TCGCards.Core
{
    public class GameField
    {
        private const int StartingHandsize = 7;
        private const int PriceCards = 1;
        private Action<GameField, List<ICard>> deckSearchAction;

        public event EventHandler<DeckSearchEventHandler> OnDeckSearchTriggered;

        public GameField()
        {
            Players = new List<Player>();
            StateQueue = new Queue<GameFieldState>();
        }

        public void Init()
        {
            Players.Add(new Player());
            Players.Add(new Player());
            ActivePlayer = Players[0];
        }

        public void InitTest()
        {
            Players.Add(new Player { Id = Guid.NewGuid() });
            Players.Add(new Player { Id = Guid.NewGuid() });
            ActivePlayer = Players[0];
        }

        public void TriggerDeckSearch(Player player, List<IDeckFilter> filterList, int cardCount, Action<GameField, List<ICard>> continueation)
        {
            GameState = GameFieldState.WaitingForDeckSearch;
            deckSearchAction = continueation;
            OnDeckSearchTriggered?.Invoke(this, new DeckSearchEventHandler(player, filterList, cardCount));
        }

        public void OnDeckSearched(List<ICard> selectedCards)
        {
            deckSearchAction?.Invoke(this, selectedCards);

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
                player.SetPrizeCards(PriceCards);
            }

            GameState = GameFieldState.SelectingActive;
        }

        public void Attack(Attack attack)
        {
            var damage = attack.GetDamage(ActivePlayer, NonActivePlayer);
            NonActivePlayer.ActivePokemonCard.DamageCounters += damage;

            attack.ProcessEffects(this, ActivePlayer, NonActivePlayer);

            if (!attack.NeedsPlayerInteraction)
                PostAttack();
        }

        public void PostAttack()
        {
            if(ActivePlayer.ActivePokemonCard.Ability?.TriggerType == TriggerType.Attacks)
                ActivePlayer.ActivePokemonCard.Ability?.Activate(ActivePlayer, NonActivePlayer);

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
        }

        public void SelectPrizeCard(ICard prizeCard)
        {
            if(GameState == GameFieldState.ActivePlayerSelectingPrize)
                ActivePlayer.DrawPrizeCard(prizeCard);
            else if(GameState == GameFieldState.UnActivePlayerSelectingPrize)
                NonActivePlayer.DrawPrizeCard(prizeCard);

            CheckEndTurn();
        }

        public void EvolvePokemon(ref IPokemonCard target, IPokemonCard evolution)
        {
            target.Evolve();

            evolution.SetBase(target);
            evolution.EvolvedFrom = target;
            target = evolution;
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

            GameState = GameFieldState.TurnStarting;
            ActivePlayer.DrawCards(1);

            GameState = GameFieldState.InTurn;
        }

        public void SwapActivePlayer()
        {
            ActivePlayer = Players.First(x => !x.Id.Equals(ActivePlayer.Id));
        }

        public void OnBothPlayersSelectedStarter()
        {
            GameState = GameFieldState.SelectingBench;
        }

        public GameFieldState GameState { get; set; }

        public List<Player> Players { get; set; }
        public Player ActivePlayer { get; set; }
        public Player NonActivePlayer { get { return Players.First(p => p.Id != ActivePlayer.Id); } }
        public int Mode { get; set; }
        public Queue<GameFieldState> StateQueue { get; set; }
    }
}
