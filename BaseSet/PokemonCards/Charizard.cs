using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Charizard : PokemonCard
    {
        public Charizard(Player owner) : base(owner)
        {
            PokemonName = "Charizard";
			EvolvesFrom = "//TODO: Evolve";
            Hp = 120;
            PokemonType = EnergyTypes.Fire;
            RetreatCost = 3;
            Weakness = EnergyTypes.Water;
			Resistance = EnergyTypes.Fighting;
			Stage = 2;
            Attacks = new List<Attack>
            {
				new FireSpin()
            };
			//TODO: Pokemon power
        }
    }
}
