using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Poliwag : PokemonCard
    {
        public Poliwag(Player owner) : base(owner)
        {
            PokemonName = "Poliwag";
			
            Hp = 40;
            PokemonType = EnergyTypes.Water;
            RetreatCost = 1;
            Weakness = EnergyTypes.Grass;
			Resistance = EnergyTypes.None;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new WaterGun(10)
            };
			
        }
    }
}
