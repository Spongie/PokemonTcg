using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Sandshrew : PokemonCard
    {
        public Sandshrew(Player owner) : base(owner)
        {
            PokemonName = "Sandshrew";
			
            Hp = 40;
            PokemonType = EnergyTypes.Fighting;
            RetreatCost = 1;
            Weakness = EnergyTypes.Grass;
			Resistance = EnergyTypes.Electric;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new Sandattack()
            };
			
        }
    }
}
