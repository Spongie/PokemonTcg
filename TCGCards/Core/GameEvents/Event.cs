namespace TCGCards.Core.GameEvents
{
    public class Event
    {
        public Event(GameFieldInfo gameField)
        {
            GameField = gameField;
        }

        public GameFieldInfo GameField { get; set; }
    }
}
