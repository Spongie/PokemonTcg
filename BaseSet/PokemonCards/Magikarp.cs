using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Magikarp : PokemonCard
    {
        public Magikarp(Player owner) : base(owner)
        {
            PokemonName = "Magikarp";
            Set = Singleton.Get<Set>();
            Hp = 30;
            PokemonType = EnergyTypes.Water;
            RetreatCost = 1;
            Weakness = EnergyTypes.Electric;
			Resistance = EnergyTypes.None;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new Tackle(),
				new Flail()
            };
			
        }
    }
}
