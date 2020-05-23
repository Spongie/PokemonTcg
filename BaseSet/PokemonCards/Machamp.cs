using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Machamp : PokemonCard
    {
        public Machamp(Player owner) : base(owner)
        {
            PokemonName = "Machamp";
			EvolvesFrom = "//TODO: Evolve";
            Hp = 100;
            PokemonType = EnergyTypes.Fighting;
            RetreatCost = 3;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
			Stage = 2;
            Attacks = new List<Attack>
            {
				new SeismicToss()
            };
			//TODO: Pokemon power
        }
    }
}
