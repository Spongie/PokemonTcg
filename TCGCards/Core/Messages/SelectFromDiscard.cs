using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class SelectFromDiscard : AbstractNetworkMessage
    {
        public SelectFromDiscard(int count, IDeckFilter filter) : this(count, count, filter)
        {

        }

        public SelectFromDiscard(int minCount, int maxCount, IDeckFilter filter)
        {
            MinCount = minCount;
            MaxCount = maxCount;
            Filter = filter;
            MessageType = MessageTypes.SelectFromDiscard;
        }

        public int MaxCount { get; set; }
        public int MinCount { get; set; }
        public IDeckFilter Filter { get; set; }
    }
}
