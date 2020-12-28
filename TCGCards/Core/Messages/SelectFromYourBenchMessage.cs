using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class SelectFromYourBenchMessage : AbstractNetworkMessage
    {
        public SelectFromYourBenchMessage()
        {

        }

        public SelectFromYourBenchMessage(int count) : this(count, count)
        {

        }

        public SelectFromYourBenchMessage(int maxCount, int minCount)
        {
            MinCount = minCount;
            MaxCount = maxCount;
            MessageType = MessageTypes.SelectFromYourBench;
        }

        public int MaxCount { get; set; }
        public int MinCount { get; set; }
        public string Info { get; set; }
        public IDeckFilter Filter { get; set; }
    }
}
