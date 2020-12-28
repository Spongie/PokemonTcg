using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class SelectOpponentPokemonMessage : AbstractNetworkMessage
    {
        public SelectOpponentPokemonMessage() :this(0)
        {

        }

        public SelectOpponentPokemonMessage(int count) :this(count, count)
        {
            
        }

        public SelectOpponentPokemonMessage(int minCount, int maxCount)
        {
            MessageType = MessageTypes.SelectOpponentPokemon;
            MaxCount = maxCount;
            MinCount = minCount;
        }

        public int MaxCount { get; set; }
        public int MinCount { get; set; }
        public string Info { get; set; }
        public IDeckFilter Filter { get; set; }
    }
}
