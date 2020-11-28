using NetworkingCore;

namespace TCGCards.Core.GameEvents
{
    public class PokemonRemovedFromBench : Event
    {
        public NetworkId PokemonId { get; set; }
    }
}
