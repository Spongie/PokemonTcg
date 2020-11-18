namespace TCGCards.Core.GameEvents
{
    public class EnergyCardsAttachedEvent : Event
    {
        public EnergyCardsAttachedEvent() :this(null)
        {

        }

        public EnergyCardsAttachedEvent(GameFieldInfo gameFieldInfo) :base(gameFieldInfo)
        {
            GameEvent = GameEventType.AttachesEnergy;
        }

        public EnergyCard EnergyCard { get; set; }
        public PokemonCard AttachedTo { get; set; }
    }
}
