using System;
using System.Collections.Generic;
using System.Linq;

namespace TCGCards.Core
{
    public class GameField
    {
        private const int StartingHandsize = 7;
        private const int PriceCards = 1;
        private Queue<GameFieldState> stateQueue;

        public GameField()
        {
            Players = new List<Player>();
            stateQueue = new Queue<GameFieldState>();
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
            attack.ProcessEffects(ActivePlayer, NonActivePlayer);

            CheckDeadPokemon();

            CheckEndTurn();
        }

        private void CheckDeadPokemon()
        {
            if(NonActivePlayer.ActivePokemonCard.IsDead())
            {
                stateQueue.Enqueue(GameFieldState.ActivePlayerSelectingPrize);
                stateQueue.Enqueue(GameFieldState.UnActivePlayerSelectingFromBench);
            }
            if(ActivePlayer.ActivePokemonCard.IsDead())
            {
                stateQueue.Enqueue(GameFieldState.UnActivePlayerSelectingPrize);
                stateQueue.Enqueue(GameFieldState.ActivePlayerSelectingFromBench);
            }
        }

        private void CheckEndTurn()
        {
            if(stateQueue.Any())
                GameState = stateQueue.Dequeue();
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

        public void EndTurn()
        {
            ActivePlayer.EndTurn();
            CheckDeadPokemon();

            if(stateQueue.Any())
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
    }
}
