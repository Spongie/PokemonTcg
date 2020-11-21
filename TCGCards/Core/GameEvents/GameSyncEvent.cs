namespace TCGCards.Core.GameEvents
{
    public class GameSyncEvent : Event
    {
        public GameSyncEvent()
        {
            GameEvent = GameEventType.SyncGame;
        }

        public GameField Game { get; set; }
    }
}
