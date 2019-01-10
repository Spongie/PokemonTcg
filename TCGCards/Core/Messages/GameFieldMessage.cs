using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class GameFieldMessage : AbstractNetworkMessage
    {
        public GameFieldMessage(GameField game)
        {
            Game = new ClientGameField(game);
            messageType = MessageTypes.GameUpdate;
        }

        public ClientGameField Game { get; set; }
    }
}
