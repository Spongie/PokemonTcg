using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class GameLogReloadMessage : AbstractNetworkMessage
    {
        public GameLogReloadMessage(GameLog log)
        {
            MessageType = MessageTypes.GameLogReload;
            GameLog = log;
        }

        public override NetworkMessage ToNetworkMessage(NetworkId senderId)
        {
            var message = base.ToNetworkMessage(senderId);
            message.RequiresResponse = false;

            return message;
        }

        public GameLog GameLog { get; set; }
    }
}
