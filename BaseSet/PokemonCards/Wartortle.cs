using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Wartortle : PokemonCard
    {
        public Wartortle(Player owner) : base(owner)
        {
            PokemonName = "Wartortle";
			EvolvesFrom = PokemonNames.Squirtle;
            Hp = 70;
            PokemonType = EnergyTypes.Water;
            RetreatCost = 1;
            Weakness = EnergyTypes.Electric;
			Resistance = EnergyTypes.None;
			Stage = 1;
            Attacks = new List<Attack>
            {
				new Withdraw(),
				new Bite()
            };
			
        }
    }
}
