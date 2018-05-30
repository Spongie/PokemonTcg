using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Deckfilters;

namespace TeamRocket.TrainerCards
{
    public class TheBosssWay : AbstractDeckSearcherTrainerCard
    {
        public override string GetName()
        {
            return "The Boss's way";
        }

        public override void Process(GameField game, Player caster, Player opponent)
        {
            List<ICard> selectedCards = TriggerDeckSearch(caster);
            game.ActivePlayer.DrawCardsFromDeck(selectedCards);
            game.RevealCardsTo(selectedCards, game.NonActivePlayer);
        }

        protected override List<IDeckFilter> GetDeckFilters()
        {
            return new List<IDeckFilter> { new DarkNameFilter() };
        }

        protected override int GetNumberOfCards()
        {
            return 1;
        }
    }
}
