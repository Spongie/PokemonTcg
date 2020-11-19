namespace TCGCards.Core.GameEvents
{
    public class EnergyCardsAttachedEvent : Event
    {
        public EnergyCardsAttachedEvent()
        {
            GameEvent = GameEventType.AttachesEnergy;
        }

        public EnergyCard EnergyCard { get; set; }
        public PokemonCard AttachedTo { get; set; }
    }
}
