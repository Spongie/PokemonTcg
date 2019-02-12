using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class SelectFromOpponentBench : AbstractNetworkMessage
    {
        public SelectFromOpponentBench(int count)
        {
            Count = count;
            MessageType = MessageTypes.SelectFromOpponentBench;
        }

        public int Count { get; set; }
    }
}
