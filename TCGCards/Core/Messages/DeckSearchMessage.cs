using NetworkingCore;
using System.Collections.Generic;

namespace TCGCards.Core.Messages
{
    public class DeckSearchMessage : AbstractNetworkMessage
    {
        public DeckSearchMessage()
        {

        }

        public DeckSearchMessage(Deck deck, List<IDeckFilter> filters, int cardCount)
        {
            Deck = deck;
            Filters = filters;
            CardCount = cardCount;
            PickedCards = new List<Card>();
            MessageType = MessageTypes.DeckSearch;
        }

        public Deck Deck { get; set; }
        public List<IDeckFilter> Filters { get; set; }
        public int CardCount { get; set; }
        public List<Card> PickedCards { get; set; }
    }
}
