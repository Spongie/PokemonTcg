using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.TrainerCards
{
    public class ComputerSearch : TrainerCard, IDeckSearcher
    {
        public ComputerSearch()
        {
            Name = "Computer Search";
            Description = "Discard 2 cards from your hand, search your library for any card and put it into your hand. Shuffle your deck.";
        }

        public override void Process(GameField game, Player caster, Player opponent)
        {
            GameUtils.DiscardCardsFromHand(caster, 2);

            var selectedCards = this.TriggerDeckSearch(caster);

            caster.DrawCardsFromDeck(selectedCards);
        }

        public override bool CanCast(GameField game, Player caster, Player opponent)
        {
            return caster.Hand.Count >= 3 && base.CanCast(game, caster, opponent);
        }

        public int GetNumberOfCards() => 1;

        public List<IDeckFilter> GetDeckFilters() => new List<IDeckFilter> { };
    }
}
