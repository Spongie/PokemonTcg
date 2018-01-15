using System;
using System.Collections.Generic;
using TCGCards.Core;
using TCGCards.Core.Deckfilters;

namespace TCGCards.PokemonCards.TeamRocket
{
    public class TheBosssWay : TrainerCard
    {
        public override string GetName()
        {
            return "The Boss's way";
        }

        public override void Process(GameField game, Player caster, Player opponent)
        {
            game.TriggerDeckSearch(caster, new List<IDeckFilter> { new DarkNameFilter() }, 1, OnDeckSearched);
        }

        private void OnDeckSearched(GameField game, List<ICard> pickedCards)
        {
            game.ActivePlayer.Hand.AddRange(pickedCards);
            game.ActivePlayer.Deck.Shuffle();
            game.RevealCardsTo(pickedCards, game.NonActivePlayer);
        }
    }
}
