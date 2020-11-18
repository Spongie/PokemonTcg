using NetworkingCore;

namespace TCGCards.Core.GameEvents
{
    public class PokemonAttackedEvent : Event
    {
        public PokemonAttackedEvent() :this(null)
        {

        }

        public PokemonAttackedEvent(GameFieldInfo gameFieldInfo) :base(gameFieldInfo)
        {
            GameEvent = GameEventType.PokemonAttacks;
        }

        public NetworkId Player { get; set; }
    }
}
