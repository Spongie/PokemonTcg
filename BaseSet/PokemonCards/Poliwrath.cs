using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Poliwrath : PokemonCard
    {
        public Poliwrath(Player owner) : base(owner)
        {
            PokemonName = "Poliwrath";
			EvolvesFrom = "//TODO: Evolve";
            Hp = 90;
            PokemonType = EnergyTypes.Water;
            RetreatCost = 3;
            Weakness = EnergyTypes.Grass;
			Resistance = EnergyTypes.None;
			Stage = 2;
            Attacks = new List<Attack>
            {
				new WaterGun(30),
				new Whirlpool()
            };
			
        }
    }
}
