﻿using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace NetworkingClient.Messages
{
    public class DeckSearchMessage
    {
        public DeckSearchMessage(DeckSearchEventHandler deckSearchEvent) : this(deckSearchEvent.Player, deckSearchEvent.Filters, deckSearchEvent.CardCount)
        {

        }

        public DeckSearchMessage(Player player, List<IDeckFilter> filters, int cardCount)
        {
            Player = player;
            Filters = filters;
            CardCount = cardCount;
            PickedCards = new List<ICard>();
        }

        public Player Player { get; }
        public List<IDeckFilter> Filters { get; }
        public int CardCount { get; }
        public List<ICard> PickedCards { get; set; }
    }
}
