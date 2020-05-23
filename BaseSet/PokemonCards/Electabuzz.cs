using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Electabuzz : PokemonCard
    {
        public Electabuzz(Player owner) : base(owner)
        {
            PokemonName = "Electabuzz";
			
            Hp = 70;
            PokemonType = EnergyTypes.Electric;
            RetreatCost = 2;
            Weakness = EnergyTypes.Fighting;
			Resistance = EnergyTypes.None;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new Thundershock(),
				new Thunderpunch()
            };
			
        }
    }
}
