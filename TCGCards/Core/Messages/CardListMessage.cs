using NetworkingCore;
using System.Collections.Generic;

namespace TCGCards.Core.Messages
{
    public class CardListMessage : AbstractNetworkMessage
    {
        public CardListMessage(List<NetworkId> cards)
        {
            Cards = new List<NetworkId>(cards);
        }

        public List<NetworkId> Cards { get; set; }
    }
}
