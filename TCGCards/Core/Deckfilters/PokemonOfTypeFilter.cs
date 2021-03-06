﻿using Entities;
using System.Linq;

namespace TCGCards.Core.Deckfilters
{
    public class PokemonOfTypeFilter : IDeckFilter
    {
        public PokemonOfTypeFilter(EnergyTypes[] types)
        {
            Types = types;
        }

        public EnergyTypes[] Types { get; set; }

        public bool IsCardValid(Card card)
        {
            var pokemon = card as PokemonCard;

            return pokemon != null && Types.Contains(pokemon.Type);
        }
    }
}
