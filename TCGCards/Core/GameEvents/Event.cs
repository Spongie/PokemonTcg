namespace TCGCards.Core.GameEvents
{
    public class Event
    {
        public Event(GameFieldInfo gameField)
        {
            GameField = gameField;
        }

        public virtual Card GetCardToDisplay()
        {
            return null;
        }

        public GameFieldInfo GameField { get; set; }
    }
}
