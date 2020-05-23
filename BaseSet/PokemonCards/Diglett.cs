using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Diglett : PokemonCard
    {
        public Diglett(Player owner) : base(owner)
        {
            PokemonName = "Diglett";
			
            Hp = 30;
            PokemonType = EnergyTypes.Fighting;
            RetreatCost = 0;
            Weakness = EnergyTypes.Grass;
			Resistance = EnergyTypes.Electric;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new Dig(),
				new MudSlap()
            };
			
        }
    }
}
