using NetworkingCore;

namespace TCGCards.Core.GameEvents
{
    public class AttachedEnergyDiscardedEvent : Event
    {
        public AttachedEnergyDiscardedEvent()
        {
            GameEvent = GameEventType.EnergyCardDiscarded;
        }

        public NetworkId FromPokemonId { get; set; }
        public EnergyCard DiscardedCard { get; set; }
    }
}
