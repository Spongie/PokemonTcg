using NetworkingCore;
using System.Collections.Generic;

namespace TCGCards.Core.Messages
{
    public class CardListMessage : AbstractNetworkMessage
    {
        public CardListMessage(List<Card> cards)
        {
            Cards = new List<Card>(cards);
        }

        public List<Card> Cards { get; set; }
    }
}
