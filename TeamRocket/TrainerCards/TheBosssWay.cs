using NetworkingCore;
using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Deckfilters;

namespace TeamRocket.TrainerCards
{
    public class TheBosssWay : TrainerCard, IDeckSearcher
    {
        public TheBosssWay()
        {
            Name = "The Boss's way";
            Description = "Search your deck for en Evolution card with Dark in its name. Show it to your opponent and put it into your hand. Shuffle your deck afterward";
        }

        public override void Process(GameField game, Player caster, Player opponent)
        {
            List<NetworkId> selectedCards = this.TriggerDeckSearch(caster);
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
