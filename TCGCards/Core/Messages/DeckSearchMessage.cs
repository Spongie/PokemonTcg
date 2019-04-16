using NetworkingCore;
using System.Collections.Generic;

namespace TCGCards.Core.Messages
{
    public class DeckSearchMessage : AbstractNetworkMessage
    {
        public DeckSearchMessage(Deck deck, List<IDeckFilter> filters, int cardCount)
        {
            Deck = deck;
            Filters = filters;
            CardCount = cardCount;
            PickedCards = new List<Card>();
            MessageType = MessageTypes.DeckSearch;
        }

        public Deck Deck { get; }
        public List<IDeckFilter> Filters { get; }
        public int CardCount { get; }
        public List<Card> PickedCards { get; set; }
    }
}
