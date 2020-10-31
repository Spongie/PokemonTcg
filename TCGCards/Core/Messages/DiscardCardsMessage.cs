using NetworkingCore;
using System.Collections.Generic;
using System.Linq;

namespace TCGCards.Core.Messages
{
    public class DiscardCardsMessage : AbstractNetworkMessage
    {
        public DiscardCardsMessage()
        {

        }

        public DiscardCardsMessage(int count) :this(count, new IDeckFilter[] { })
        {

        }

        public DiscardCardsMessage(int count, IEnumerable<IDeckFilter> filters)
        {
            MessageType = MessageTypes.DiscardCards;
            Count = count;
            Filters = filters.ToList();
        }

        public int Count { get; set; }
        public List<IDeckFilter> Filters { get; set; }
    }
}
