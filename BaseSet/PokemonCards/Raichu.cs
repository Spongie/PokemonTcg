using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Raichu : PokemonCard
    {
        public Raichu(Player owner) : base(owner)
        {
            PokemonName = "Raichu";
			EvolvesFrom = "//TODO: Evolve";
            Hp = 80;
            PokemonType = EnergyTypes.Electric;
            RetreatCost = 1;
            Weakness = EnergyTypes.Fighting;
			Resistance = EnergyTypes.None;
			Stage = 1;
            Attacks = new List<Attack>
            {
				new Agility(),
				new Thunder()
            };
			
        }
    }
}
