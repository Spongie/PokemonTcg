using NetworkingCore;

namespace TCGCards.Core.GameEvents
{
    public class PokemonAddedToBenchEvent : Event
    {
        public PokemonAddedToBenchEvent()
        {
            GameEvent = GameEventType.PokemonAddedToBench;
        }

        public PokemonCard Pokemon { get; set; }
        public NetworkId Player { get; set; }
    }
}
