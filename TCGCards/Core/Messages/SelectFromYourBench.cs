using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class SelectFromYourBench : AbstractNetworkMessage
    {
        public SelectFromYourBench()
        {

        }

        public SelectFromYourBench(int count) : this(count, count)
        {

        }

        public SelectFromYourBench(int maxCount, int minCount)
        {
            MinCount = minCount;
            MaxCount = maxCount;
            MessageType = MessageTypes.SelectFromYourBench;
        }

        public int MaxCount { get; set; }
        public int MinCount { get; set; }
    }
}
