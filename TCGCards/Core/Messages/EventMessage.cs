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

        public override NetworkMessage ToNetworkMessage(NetworkId senderId)
        {
            var message = base.ToNetworkMessage(senderId);
            message.RequiresResponse = false;

            return message;
        }

        public Event GameEvent { get; set; }
    }
}
