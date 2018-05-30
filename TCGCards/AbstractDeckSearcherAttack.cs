using System;
using System.Collections.Generic;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards
{
    public abstract class AbstractDeckSearcherAttack : Attack
    {
        protected abstract int GetNumberOfCards();
        protected abstract List<IDeckFilter> GetDeckFilters();

        protected List<ICard> TriggerDeckSearch(Player owner)
        {
            var message = new DeckSearchMessage(owner, GetDeckFilters(), GetNumberOfCards());
            var response = owner.NetworkPlayer.SendAndWaitForResponse<DeckSearchedMessage>(message.ToNetworkMessage(owner.Id));

            if (response.SelectedCards.Count != GetNumberOfCards())
                throw new Exception("Cheating!?");

            owner.Deck.Shuffle();
            return response.SelectedCards;
        }
    }
}
