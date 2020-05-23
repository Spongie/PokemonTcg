using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Chansey : PokemonCard
    {
        public Chansey(Player owner) : base(owner)
        {
            PokemonName = "Chansey";
			
            Hp = 120;
            PokemonType = EnergyTypes.Colorless;
            RetreatCost = 1;
            Weakness = EnergyTypes.Fighting;
			Resistance = EnergyTypes.Psychic;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new Scrunch(),
				new Doubleedge()
            };
			
        }
    }
}
