using Entities;
using System.Linq;

namespace TCGCards.Core.Deckfilters
{
    public class PokemonWithNameOrTypeFilter : IDeckFilter
    {
        public PokemonWithNameOrTypeFilter()
        {

        }

        public PokemonWithNameOrTypeFilter(string names, EnergyTypes type) :this(names, type, false)
        {

        }

        public PokemonWithNameOrTypeFilter(string names, EnergyTypes type, bool onlyBasic)
        {
            Names = names;
            Type = type;
            OnlyBasic = onlyBasic;
        }

        public string Names { get; set; }
        public EnergyTypes Type { get; set; }
        public bool OnlyBasic { get; set; }
        public bool InvertName { get; set; }

        public bool IsCardValid(Card card)
        {
            bool valid = true;
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
                if (!InvertName)
                {
                    valid = Names.Split(';').Contains(card.Name);
                }
                else
                {
                    valid = !Names.Split(';').Contains(card.Name);
                }
            }

            if (OnlyBasic && pokemon.Stage > 0)
            {
                valid = false;
            }

            return valid;
        }
    }
}
