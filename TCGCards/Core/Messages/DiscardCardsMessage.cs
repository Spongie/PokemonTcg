using NetworkingCore;
using System.Collections.Generic;

namespace TCGCards.Core.Messages
{
    public class DiscardCardsMessage : AbstractNetworkMessage
    {
        public DiscardCardsMessage()
        {

        }

        public DiscardCardsMessage(int count) :this(count, new List<IDeckFilter>())
        {

        }

        public DiscardCardsMessage(int count, List<IDeckFilter> filters)
        {
            MessageType = MessageTypes.DiscardCards;
            Count = count;
            Filters = new List<IDeckFilter>(filters);
        }

        public int Count { get; set; }
        public List<IDeckFilter> Filters { get; set; }
        public string Info { get; set; }
    }
}
