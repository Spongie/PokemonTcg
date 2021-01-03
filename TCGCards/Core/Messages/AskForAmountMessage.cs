using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class AskForAmountMessage : AbstractNetworkMessage
    {
        public AskForAmountMessage()
        {
            MessageType = MessageTypes.AskForAmount;
        }

        public string Info { get; set; }
        public int Answer { get; set; }
    }
}
