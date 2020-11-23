using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class InfoMessage : AbstractNetworkMessage
    {
        public InfoMessage()
        {
            MessageType = MessageTypes.Info;
        }

        public InfoMessage(string info) :this()
        {
            Info = info;
        }

        public string Info { get; set; }

        public override NetworkMessage ToNetworkMessage(NetworkId senderId)
        {
            var message = base.ToNetworkMessage(senderId);
            message.RequiresResponse = false;

            return message;
        }
    }
}
