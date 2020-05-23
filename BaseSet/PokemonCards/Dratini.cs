using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Dratini : PokemonCard
    {
        public Dratini(Player owner) : base(owner)
        {
            PokemonName = "Dratini";
			
            Hp = 40;
            PokemonType = EnergyTypes.Colorless;
            RetreatCost = 1;
            Weakness = EnergyTypes.None;
			Resistance = EnergyTypes.Psychic;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new Pound()
            };
			
        }
    }
}
