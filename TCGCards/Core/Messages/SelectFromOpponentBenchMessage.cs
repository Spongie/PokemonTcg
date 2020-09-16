using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class SelectFromOpponentBenchMessage : AbstractNetworkMessage
    {
        public SelectFromOpponentBenchMessage()
        {

        }

        public SelectFromOpponentBenchMessage(int count) : this(count, count)
        {

        }

        public SelectFromOpponentBenchMessage(int minCount, int maxCount)
        {
            MinCount = minCount;
            MaxCount = maxCount;
            MessageType = MessageTypes.SelectFromOpponentBench;
        }

        public int MaxCount { get; set; }
        public int MinCount { get; set; }
    }
}
