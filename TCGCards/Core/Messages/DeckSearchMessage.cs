﻿using NetworkingCore;
using System.Collections.Generic;

namespace TCGCards.Core.Messages
{
    public class DeckSearchMessage : AbstractNetworkMessage
    {
        public DeckSearchMessage()
        {

        }

        public DeckSearchMessage(List<Card> cards, List<IDeckFilter> filters, int cardCount)
        {
            Cards = cards;
            Filters = filters;
            CardCount = cardCount;
            PickedCards = new List<Card>();
            MessageType = MessageTypes.DeckSearch;
        }

        public List<Card> Cards { get; set; }
        public List<IDeckFilter> Filters { get; set; }
        public int CardCount { get; set; }
        public List<Card> PickedCards { get; set; }
    }
}
