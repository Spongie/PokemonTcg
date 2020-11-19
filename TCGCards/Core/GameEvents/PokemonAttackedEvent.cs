using NetworkingCore;

namespace TCGCards.Core.GameEvents
{
    public class PokemonAttackedEvent : Event
    {
        public PokemonAttackedEvent()
        {
            GameEvent = GameEventType.PokemonAttacks;
        }

        public NetworkId Player { get; set; }
    }
}
