using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class SelectFromYourBench : AbstractNetworkMessage
    {
        public SelectFromYourBench(int count)
        {
            Count = count;
            MessageType = MessageTypes.SelectFromYourBench;
        }

        public int Count { get; set; }
    }
}
