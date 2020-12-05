using NetworkingCore;

namespace TCGCards.Core.GameEvents
{
    public class PokemonHealedEvent : Event
    {
        public PokemonHealedEvent()
        {
            GameEvent = GameEventType.PokemonHealed;
        }

        public NetworkId PokemonId { get; set; }
        public int Healing { get; set; }
    }
}
