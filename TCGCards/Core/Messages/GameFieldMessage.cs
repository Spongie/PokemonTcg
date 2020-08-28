using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class GameFieldMessage : AbstractNetworkMessage
    {
        public GameFieldMessage()
        {

        }

        public GameFieldMessage(GameField game)
        {
            Game = game;
            MessageType = MessageTypes.GameUpdate;
        }

        public GameField Game { get; set; }

        public override NetworkMessage ToNetworkMessage(NetworkId senderId)
        {
            var message = base.ToNetworkMessage(senderId);
            message.RequiresResponse = false;

            return message;
        }
    }
}
