namespace TCGCards.Core.GameEvents
{
    public class StadiumDestroyedEvent : Event
    {
        public StadiumDestroyedEvent()
        {
            GameEvent = GameEventType.StadiumCardDestroyed;
        }
    }
}
