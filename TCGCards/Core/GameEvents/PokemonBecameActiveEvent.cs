using NetworkingCore;

namespace TCGCards.Core.GameEvents
{
    public class PokemonBecameActiveEvent : Event
    {
        public PokemonBecameActiveEvent()
        {
            GameEvent = GameEventType.PokemonBecameActive;
        }

        public NetworkId NewActivePokemonId { get; set; }
        public NetworkId ReplacedPokemonId { get; set; }
        public PokemonCard NewActivePokemon { get; set; }
    }
}
