using NetworkingCore;

namespace TCGCards.Core.GameEvents
{
    public class PokemonBouncedEvent : Event
    {
        public PokemonBouncedEvent()
        {
            GameEvent = GameEventType.PokemonBounced;
        }

        public NetworkId PokemonId { get; set; }
        public bool ToHand { get; set; }
    }
}
