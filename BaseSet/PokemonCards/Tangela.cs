using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Tangela : PokemonCard
    {
        public Tangela(Player owner) : base(owner)
        {
            PokemonName = "Tangela";
			
            Hp = 50;
            PokemonType = EnergyTypes.Grass;
            RetreatCost = 2;
            Weakness = EnergyTypes.Fire;
			Resistance = EnergyTypes.None;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new Bind(),
				new Poisonpowder()
            };
			
        }
    }
}
