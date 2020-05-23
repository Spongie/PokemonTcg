using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Charmander : PokemonCard
    {
        public Charmander(Player owner) : base(owner)
        {
            PokemonName = "Charmander";
			
            Hp = 50;
            PokemonType = EnergyTypes.Fire;
            RetreatCost = 1;
            Weakness = EnergyTypes.Water;
			Resistance = EnergyTypes.None;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new Scratch(),
				new Ember()
            };
			
        }
    }
}
