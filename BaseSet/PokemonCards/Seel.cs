using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Seel : PokemonCard
    {
        public Seel(Player owner) : base(owner)
        {
            PokemonName = "Seel";
			
            Hp = 60;
            PokemonType = EnergyTypes.Water;
            RetreatCost = 1;
            Weakness = EnergyTypes.Electric;
			Resistance = EnergyTypes.None;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new Headbutt()
            };
			
        }
    }
}
