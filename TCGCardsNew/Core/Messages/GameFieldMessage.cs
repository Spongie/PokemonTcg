using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class GameFieldMessage : AbstractNetworkMessage
    {
        public GameFieldMessage(GameField game)
        {
            Game = game;
            messageType = MessageTypes.GameUpdate;
        }

        public GameField Game { get; set; }
    }
}
