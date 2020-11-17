namespace TCGCards.Core.GameEvents
{
    public class Event
    {
        public Event(GameFieldInfo gameField)
        {
            GameField = gameField;
        }

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
    }
}
