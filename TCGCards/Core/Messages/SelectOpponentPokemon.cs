using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class SelectOpponentPokemon : AbstractNetworkMessage
    {
        public SelectOpponentPokemon(int count)
        {
            MessageType = MessageTypes.SelectOpponentPokemon;
            Count = count;
        }

        public int Count { get; set; }
    }
}
