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
    }
}
