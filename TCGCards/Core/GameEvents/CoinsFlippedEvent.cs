using System.Collections.Generic;

namespace TCGCards.Core.GameEvents
{
    public class CoinsFlippedEvent : Event
    {
        public CoinsFlippedEvent() :this(new List<bool>())
        {

        }

        public CoinsFlippedEvent(List<bool> results)
        {
            Results = results;
            GameEvent = GameEventType.Flipscoin;
        }

        public List<bool> Results { get; set; }
    }
}
