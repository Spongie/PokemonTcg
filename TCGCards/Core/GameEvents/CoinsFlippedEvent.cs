using System.Collections.Generic;

namespace TCGCards.Core.GameEvents
{
    public class CoinsFlippedEvent : Event
    {
        public CoinsFlippedEvent() :this(null, null)
        {

        }

        public CoinsFlippedEvent(List<bool> results, GameFieldInfo gameFieldInfo) :base(gameFieldInfo)
        {
            Results = results;
            GameEvent = GameEventType.Flipscoin;
        }

        public List<bool> Results { get; set; }
    }
}
