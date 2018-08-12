﻿using System;
using System.Collections.Generic;
using TCGCards.Core.Messages;

namespace TCGCards.Core
{
    public static class DeckSearcherExtensions
    {
        public static List<ICard> TriggerDeckSearch(this IDeckSearcher deckSearcher, Player owner)
        {
            var message = new DeckSearchMessage(owner, deckSearcher.GetDeckFilters(), deckSearcher.GetNumberOfCards());
            var response = owner.NetworkPlayer.SendAndWaitForResponse<DeckSearchedMessage>(message.ToNetworkMessage(owner.Id));

            if (response.SelectedCards.Count != deckSearcher.GetNumberOfCards())
                throw new Exception("Cheating!?");

            owner.Deck.Shuffle();
            return response.SelectedCards;
        }
    }
}