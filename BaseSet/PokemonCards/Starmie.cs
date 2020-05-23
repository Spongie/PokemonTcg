using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Starmie : PokemonCard
    {
        public Starmie(Player owner) : base(owner)
        {
            PokemonName = "Starmie";
			EvolvesFrom = "//TODO: Evolve";
            Hp = 60;
            PokemonType = EnergyTypes.Water;
            RetreatCost = 1;
            Weakness = EnergyTypes.Electric;
			Resistance = EnergyTypes.None;
			Stage = 1;
            Attacks = new List<Attack>
            {
				new Recover(),
				new StarFreeze()
            };
			
        }
    }
}
