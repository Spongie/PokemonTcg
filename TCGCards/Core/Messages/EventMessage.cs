using NetworkingCore;
using TCGCards.Core.GameEvents;

namespace TCGCards.Core.Messages
{
    public class EventMessage : AbstractNetworkMessage
    {
        public EventMessage()
        {
            MessageType = MessageTypes.GameEvent;
        }

        public EventMessage(Event gameEvent) :this()
        {
            GameEvent = gameEvent;
        }

        public Event GameEvent { get; set; }
    }
}
