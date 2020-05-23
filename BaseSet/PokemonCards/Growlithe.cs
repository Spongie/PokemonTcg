using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Growlithe : PokemonCard
    {
        public Growlithe(Player owner) : base(owner)
        {
            PokemonName = "Growlithe";
			
            Hp = 60;
            PokemonType = EnergyTypes.Fire;
            RetreatCost = 1;
            Weakness = EnergyTypes.Water;
			Resistance = EnergyTypes.None;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new Flare()
            };
			
        }
    }
}
