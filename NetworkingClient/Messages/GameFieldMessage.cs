using TCGCards.Core;

namespace NetworkingClient.Messages
{
    public struct GameFieldMessage
    {
        public GameFieldMessage(GameField game)
        {
            Game = game;
        }

        public GameField Game { get; set; }
    }
}
