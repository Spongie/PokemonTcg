using NetworkingCore;
using System.Collections.Generic;
using System.Linq;

namespace TCGCards.Core.Messages
{
    public class SelectAttackMessage : AbstractNetworkMessage
    {
        public SelectAttackMessage()
        {

        }

        public SelectAttackMessage(IEnumerable<Attack> availableChoices)
        {
            AvailableAttacks = availableChoices.ToList();
        }

        public List<Attack> AvailableAttacks { get; set; }
    }
}
