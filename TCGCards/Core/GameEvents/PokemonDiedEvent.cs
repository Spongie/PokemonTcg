namespace TCGCards.Core.GameEvents
{
    public class PokemonDiedEvent : Event
    {
        public PokemonDiedEvent()
        {
            GameEvent = GameEventType.PokemonDied;
        }

        public PokemonCard Pokemon { get; set; }
    }
}
