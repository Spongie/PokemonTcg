using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class GameFieldMessage : AbstractNetworkMessage
    {
        public GameFieldMessage(GameField game)
        {
            Game = game;
            MessageType = MessageTypes.GameUpdate;
        }

        public GameField Game { get; set; }
    }
}
