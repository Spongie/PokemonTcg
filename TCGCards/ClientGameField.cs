using System.Collections.Generic;
using System.Linq;
using TCGCards.Core;

namespace TCGCards
{
    public class ClientGameField
    {
        public ClientGameField(GameField gameField)
        {
            Players = new List<ClientPlayer>();

            foreach (var player in gameField.Players)
            {
                Players.Add(new ClientPlayer(player));    
            }

            ActivePlayer = gameField.ActivePlayer == null ? Players.First() : Players.First(p => p.Id.Equals(gameField.ActivePlayer.Id));
            NonActivePlayer = gameField.ActivePlayer == null ? Players.First() : Players.First(p => p.Id.Equals(gameField.NonActivePlayer.Id));
            PrizeCardsFaceUp = gameField.PrizeCardsFaceUp;
            GameState = gameField.GameState;
        }

        public List<ClientPlayer> Players { get; set; }
        public ClientPlayer ActivePlayer { get; set; }
        public ClientPlayer NonActivePlayer { get; set; }
        public bool PrizeCardsFaceUp { get; set; }
        public GameFieldState GameState { get; set; }
    }
}
