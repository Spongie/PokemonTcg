using NetworkingCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core.Messages;

namespace TCGCards.Core
{
    public static class DeckSearcherExtensions
    {
        public static List<NetworkId> TriggerDeckSearch(this IDeckSearcher deckSearcher, Player owner)
        {
            var message = new DeckSearchMessage(owner.Deck.Cards.ToList(), deckSearcher.GetDeckFilters(), deckSearcher.GetNumberOfCards());
            var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message.ToNetworkMessage(owner.Id));

            if (response.Cards.Count != deckSearcher.GetNumberOfCards())
                throw new Exception("Cheating!?");

            owner.Deck.Shuffle();
            return response.Cards;
        }
    }
}
