using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class SelectFromOpponentBench : AbstractNetworkMessage
    {
        public SelectFromOpponentBench()
        {

        }

        public SelectFromOpponentBench(int count) : this(count, count)
        {

        }

        public SelectFromOpponentBench(int minCount, int maxCount)
        {
            MinCount = minCount;
            MaxCount = maxCount;
            MessageType = MessageTypes.SelectFromOpponentBench;
        }

        public int MaxCount { get; set; }
        public int MinCount { get; set; }
    }
}
