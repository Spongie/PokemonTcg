using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkMagneton : IPokemonCard
    {
        public DarkMagneton(Player owner) : base(owner)
        {
            PokemonName = "Dark Magneton";
			EvolvesFrom = "Magnemite"; //TODO: Add stage
            Hp = 60;
            PokemonType = EnergyTypes.Electric;
            RetreatCost = 2;
            Weakness = EnergyTypes.Fighting;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new Sonicboom(),
				new MagneticLines()
            };
			
        }
    }
}
