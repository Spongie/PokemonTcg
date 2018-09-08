using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class DiscardCardsMessage : AbstractNetworkMessage
    {
        public DiscardCardsMessage(int count)
        {
            Count = count;
        }

        public int Count { get; set; }
    }
}
