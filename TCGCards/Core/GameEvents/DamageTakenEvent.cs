using Entities;
using NetworkingCore;

namespace TCGCards.Core.GameEvents
{
    public class DamageTakenEvent : Event
    {
        public DamageTakenEvent()
        {
            GameEvent = GameEventType.PokemonTakesDamage;
        }

        public NetworkId PokemonId { get; set; }
        public int Damage { get; set; }
        public EnergyTypes DamageType { get; set; }
    }
}
