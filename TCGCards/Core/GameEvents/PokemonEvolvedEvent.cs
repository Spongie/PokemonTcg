using NetworkingCore;

namespace TCGCards.Core.GameEvents
{
    public class PokemonEvolvedEvent : Event
    {
        public PokemonEvolvedEvent()
        {
            GameEvent = GameEventType.PokemonEvolved;
        }

        public NetworkId TargetPokemonId { get; set; }
        public PokemonCard NewPokemonCard { get; set; }
    }
}
