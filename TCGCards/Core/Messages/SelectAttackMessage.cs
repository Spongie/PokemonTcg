using NetworkingCore;
using System.Collections.Generic;
using System.Linq;

namespace TCGCards.Core.Messages
{
    public class SelectAttackMessage : AbstractNetworkMessage
    {
        public SelectAttackMessage()
        {
            MessageType = MessageTypes.SelectAttack;
        }

        public SelectAttackMessage(List<Attack> availableChoices) :this()
        {
            AvailableAttacks = new List<Attack>(availableChoices);
        }

        public List<Attack> AvailableAttacks { get; set; }
    }
}
