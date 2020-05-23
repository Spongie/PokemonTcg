using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Gastly : PokemonCard
    {
        public Gastly(Player owner) : base(owner)
        {
            PokemonName = "Gastly";
			
            Hp = 30;
            PokemonType = EnergyTypes.Psychic;
            RetreatCost = 0;
            Weakness = EnergyTypes.None;
			Resistance = EnergyTypes.Fighting;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new SleepingGas(),
				new DestinyBond()
            };
			
        }
    }
}
