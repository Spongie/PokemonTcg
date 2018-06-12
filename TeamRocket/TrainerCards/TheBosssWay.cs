using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Deckfilters;

namespace TeamRocket.TrainerCards
{
    public class TheBosssWay : TrainerCard, IDeckSearcher
    {
        public override string GetName()
        {
            return "The Boss's way";
        }

        public override void Process(GameField game, Player caster, Player opponent)
        {
            List<ICard> selectedCards = this.TriggerDeckSearch(caster);
            game.ActivePlayer.DrawCardsFromDeck(selectedCards);
            game.RevealCardsTo(selectedCards, game.NonActivePlayer);
        }

        public List<IDeckFilter> GetDeckFilters()
        {
            return new List<IDeckFilter> { new DarkNameFilter() };
        }

        public int GetNumberOfCards()
        {
            return 1;
        }
    }
}
