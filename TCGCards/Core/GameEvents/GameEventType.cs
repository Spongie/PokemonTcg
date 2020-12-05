namespace TCGCards.Core.GameEvents
{
    public enum GameEventType
    {
        TrainerCardPlayed = 0,
        PokemonAttacks = 1,
        PokemonActivatesAbility = 2,
        PokemonTakesDamage = 3,
        DrawsCard = 4,
        DiscardsCard = 5,
        Flipscoin = 6,
        AttachesEnergy = 7,
        PokemonBecameActive = 8,
        PokemonAddedToBench = 9,
        PokemonEvolved = 10,
        EnergyCardDiscarded = 11,
        PokemonDied = 12,
        SyncGame = 13,
        GameInfo = 14,
        PokemonBounced = 15,
        PokemonHealed = 16
    }
}
