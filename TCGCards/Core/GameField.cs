using System;
using System.Collections.Generic;
using System.Linq;

namespace TCGCards.Core
{
    public class GameField
    {
        public GameField()
        {
            Players = new List<Player>();
            Players.Add(new Player());
            Players.Add(new Player());
            ActivePlayer = Players[0];
        }

        public void StartGame()
        {
            ActivePlayer = Players[new Random().Next(2)];

            foreach(var player in Players)
            {
                player.Deck.Shuffle();
            }

            GameState = GameFieldState.SelectingActive;
        }

        public void EndTurn()
        {
            ActivePlayer.EndTurn();
            SwapActivePlayer();

            StartNextTurn();
        }

        private void StartNextTurn()
        {
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
        public int Mode { get; set; }
    }
}
