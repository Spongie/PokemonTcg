using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Magnemite : PokemonCard
    {
        public Magnemite(Player owner) : base(owner)
        {
            PokemonName = "Magnemite";
			
            Hp = 40;
            PokemonType = EnergyTypes.Electric;
            RetreatCost = 1;
            Weakness = EnergyTypes.Fighting;
			Resistance = EnergyTypes.None;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new ThunderWave(),
				new Selfdestruct()
            };
			
        }
    }
}
