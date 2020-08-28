using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class GameOverMessage : AbstractNetworkMessage
    {
        public GameOverMessage()
        {

        }

        public GameOverMessage(NetworkId winner)
        {
            WinnerId = winner;
            MessageType = MessageTypes.GameOver;
        }

        public NetworkId WinnerId { get; set; }

        public override NetworkMessage ToNetworkMessage(NetworkId senderId)
        {
            var message = base.ToNetworkMessage(senderId);
            message.RequiresResponse = false;

            return message;
        }
    }
}
