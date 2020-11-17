using NetworkingCore;
using TCGCards.Core.GameEvents;

namespace TCGCards.Core.Messages
{
    public class GameEventMessage : AbstractNetworkMessage
    {
        public GameEventMessage()
        {

        }

        public GameEventMessage(Event gameEvent)
        {
            GameEvent = gameEvent;
        }

        public Event GameEvent { get; set; }
    }
}
