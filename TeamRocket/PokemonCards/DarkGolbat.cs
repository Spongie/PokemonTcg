using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkGolbat : IPokemonCard
    {
        public DarkGolbat(Player owner) : base(owner)
        {
            PokemonName = "Dark Golbat";
			EvolvesFrom = "Zubat"; //TODO: Add stage
            Hp = 50;
            PokemonType = EnergyTypes.Grass;
            RetreatCost = 0;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.Fighting;
            Attacks = new List<Attack>
            {
				new Flitter()
            };
			//TODO: Pokemon power
        }
    }
}
