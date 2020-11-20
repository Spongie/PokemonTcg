namespace TCGCards.Core.GameEvents
{
    public class Event
    {
        public GameEventType GameEvent { get; set; }
        public GameFieldInfo GameField { get; set; }
    }

    public enum GameEventType
    {
        TrainerCardPlayed,
        PokemonAttacks,
        PokemonActivatesAbility,
        PokemonTakesDamage,
        DrawsCard,
        DiscardsCard,
        Flipscoin,
        AttachesEnergy,
        PokemonBecameActive,
        PokemonAddedToBench,
        PokemonEvolved, //Continue on this one
        EnergyCardDiscarded,
        PokemonDied
    }
}
