namespace TCGCards.Core.GameEvents
{
    public class GameUpdateEvent : Event
    {
        public GameUpdateEvent(GameFieldInfo gameField) : base(gameField)
        {
        }
    }
}
