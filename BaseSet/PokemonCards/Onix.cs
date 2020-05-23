using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Onix : PokemonCard
    {
        public Onix(Player owner) : base(owner)
        {
            PokemonName = "Onix";
			
            Hp = 90;
            PokemonType = EnergyTypes.Fighting;
            RetreatCost = 3;
            Weakness = EnergyTypes.Grass;
			Resistance = EnergyTypes.None;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new RockThrow(),
				new Harden()
            };
			
        }
    }
}
