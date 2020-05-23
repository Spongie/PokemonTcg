using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Vulpix : PokemonCard
    {
        public Vulpix(Player owner) : base(owner)
        {
            PokemonName = "Vulpix";
			
            Hp = 50;
            PokemonType = EnergyTypes.Fire;
            RetreatCost = 1;
            Weakness = EnergyTypes.Water;
			Resistance = EnergyTypes.None;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new ConfuseRay()
            };
			
        }
    }
}
