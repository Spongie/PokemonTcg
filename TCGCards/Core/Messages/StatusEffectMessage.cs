using Entities;
using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class StatusEffectMessage : AbstractNetworkMessage
    {
        public StatusEffectMessage()
        {
            MessageType = MessageTypes.StatusSelected;
        }

        public StatusEffect StatusEffect { get; set; }
    }
}
