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

        public SelectAttackMessage(IEnumerable<Attack> availableChoices) :this()
        {
            AvailableAttacks = availableChoices.ToList();
        }

        public List<Attack> AvailableAttacks { get; set; }
    }
}
