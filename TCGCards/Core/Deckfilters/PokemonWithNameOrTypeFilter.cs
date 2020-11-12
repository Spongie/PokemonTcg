using Entities;
using System.Linq;

namespace TCGCards.Core.Deckfilters
{
    public class PokemonWithNameOrTypeFilter : IDeckFilter
    {
        public PokemonWithNameOrTypeFilter()
        {

        }

        public PokemonWithNameOrTypeFilter(string names, EnergyTypes type)
        {
            Names = names;
            Type = type;
        }

        public string Names { get; set; }
        public EnergyTypes Type { get; set; }

        public bool IsCardValid(Card card)
        {
            bool valid = false;
            var pokemon = card as PokemonCard;

            if (pokemon == null)
            {
                return false;
            }

            if (Type != EnergyTypes.All && Type != EnergyTypes.None)
            {
                valid = pokemon.Type == Type;
            }
            
            if (valid && !string.IsNullOrEmpty(Names))
            {
                valid = Names.Split(';').Contains(card.Name);
            }

            return valid;
        }
    }
}
