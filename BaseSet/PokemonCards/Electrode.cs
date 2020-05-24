using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Electrode : PokemonCard
    {
        public Electrode(Player owner) : base(owner)
        {
            PokemonName = "Electrode";
			EvolvesFrom = PokemonNames.Electrode;
            Hp = 80;
            PokemonType = EnergyTypes.Electric;
            RetreatCost = 1;
            Weakness = EnergyTypes.Fighting;
			Resistance = EnergyTypes.None;
			Stage = 1;
            Attacks = new List<Attack>
            {
				new ElectricShock()
            };
			//TODO: Pokemon power
        }
    }
}
