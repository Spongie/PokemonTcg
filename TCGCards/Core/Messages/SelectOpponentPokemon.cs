using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class SelectOpponentPokemon : AbstractNetworkMessage
    {
        public SelectOpponentPokemon(int count) :this(count, count)
        {
            
        }

        public SelectOpponentPokemon(int minCount, int maxCount)
        {
            MessageType = MessageTypes.SelectOpponentPokemon;
            MaxCount = maxCount;
            MinCount = minCount;
        }

        public int MaxCount { get; set; }
        public int MinCount { get; set; }
    }
}
