using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class DiscardCardsMessage : AbstractNetworkMessage
    {
        public DiscardCardsMessage(int count)
        {
            MessageType = MessageTypes.DiscardCards;
            Count = count;
        }

        public int Count { get; set; }
    }
}
