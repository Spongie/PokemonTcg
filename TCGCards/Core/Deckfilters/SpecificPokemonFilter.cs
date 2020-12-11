using NetworkingCore;
using System.Collections.Generic;

namespace TCGCards.Core.Deckfilters
{
    public class PokemonWithNameAndOwner : IDeckFilter
    {
        public NetworkId Owner { get; set; }
        public List<string> Names { get; set; }

        public bool IsCardValid(Card card)
        {
            var pokemon = card as PokemonCard;

            return pokemon != null && card.Owner.Id.Equals(Owner) && Names.Contains(pokemon.PokemonName);
        }
    }
}
