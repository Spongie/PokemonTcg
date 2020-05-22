using System;
using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Abilities
{
    public class SummonMinions : Ability, IDeckSearcher
    {
        private int searchCount;

        public SummonMinions(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.EnterPlay;
            Name = "Summon Minions";
            Description = "When you play Dark Dragonite from your hand search your deck for up to 2 basic pokemon and put them onto your bench";
        }

        public List<IDeckFilter> GetDeckFilters() => new List<IDeckFilter> { new Filter() };

        public int GetNumberOfCards() => searchCount;

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameLog log)
        {
            searchCount = Math.Min(5 - owner.BenchedPokemon.Count, 2);

            if (searchCount == 0)
                return;

            var cards = this.TriggerDeckSearch(owner);

            owner.DrawCardsFromDeck(cards);
            foreach (var card in cards.OfType<PokemonCard>())
            {
                owner.SetBenchedPokemon(card);
            }
        }

        private class Filter : IDeckFilter
        {
            public bool IsCardValid(Card card)
            {
                return card is PokemonCard && ((PokemonCard)card).Stage == 0;
            }
        }
    }
}
