using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class SelectFromOpponentBench : AbstractNetworkMessage
    {
        public SelectFromOpponentBench(int count)
        {
            Count = count;
        }

        public int Count { get; set; }
    }
}
