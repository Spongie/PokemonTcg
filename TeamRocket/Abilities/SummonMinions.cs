using System;
using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Abilities
{
    public class SummonMinions : Ability, IDeckSearcher
    {
        public SummonMinions(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.EnterPlay;
        }

        public List<IDeckFilter> GetDeckFilters() => new List<IDeckFilter> { new Filter() };

        public int GetNumberOfCards() => 1;

        protected override void Activate(Player owner, Player opponent, int damageTaken)
        {
            int limit = Math.Min(5 - owner.BenchedPokemon.Count, 2);

            if (limit == 0)
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
