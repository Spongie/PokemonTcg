using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Porygon : PokemonCard
    {
        public Porygon(Player owner) : base(owner)
        {
            PokemonName = "Porygon";
			
            Hp = 30;
            PokemonType = EnergyTypes.Colorless;
            RetreatCost = 1;
            Weakness = EnergyTypes.Fighting;
			Resistance = EnergyTypes.Psychic;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new Conversion1(),
				new Conversion2()
            };
			
        }
    }
}
