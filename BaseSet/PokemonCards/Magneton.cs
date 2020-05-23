using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Magneton : PokemonCard
    {
        public Magneton(Player owner) : base(owner)
        {
            PokemonName = "Magneton";
			EvolvesFrom = "//TODO: Evolve";
            Hp = 60;
            PokemonType = EnergyTypes.Electric;
            RetreatCost = 1;
            Weakness = EnergyTypes.Fighting;
			Resistance = EnergyTypes.None;
			Stage = 1;
            Attacks = new List<Attack>
            {
				new ThunderWave(),
				new Selfdestruct()
            };
			
        }
    }
}
