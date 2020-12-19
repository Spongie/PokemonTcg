using NetworkingCore;

namespace TCGCards.Core.GameEvents
{
    public class PokemonRemovedFromBench : Event
    {
        public PokemonRemovedFromBench()
        {
            GameEvent = GameEventType.RemovedFromBench;
        }

        public NetworkId PokemonId { get; set; }
    }
}
