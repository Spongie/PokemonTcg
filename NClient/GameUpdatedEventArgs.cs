using System;
using TCGCards.Core;

namespace NetworkingClient
{
    public class GameUpdatedEventArgs : EventArgs
    {
        public GameUpdatedEventArgs(GameField game)
        {
            Game = game;
        }

        public GameField Game { get; set; }
    }
}
