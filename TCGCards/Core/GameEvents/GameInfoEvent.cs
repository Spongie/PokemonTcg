namespace TCGCards.Core.GameEvents
{
    public class GameInfoEvent : Event
    {
        public GameInfoEvent()
        {
            GameEvent = GameEventType.GameInfo;
        }
    }
}
