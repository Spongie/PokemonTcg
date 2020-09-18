using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Squirtle : PokemonCard
    {
        public Squirtle(Player owner) : base(owner)
        {
            PokemonName = "Squirtle";
            Set = Singleton.Get<Set>();
            Hp = 40;
            PokemonType = EnergyTypes.Water;
            RetreatCost = 1;
            Weakness = EnergyTypes.Electric;
			Resistance = EnergyTypes.None;
			Stage = 0;
            Attacks = new List<Attack>
            {
				new Bubble(),
				new Withdraw()
            };
			
        }
    }
}
