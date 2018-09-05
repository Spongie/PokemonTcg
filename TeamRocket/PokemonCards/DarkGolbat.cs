using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkGolbat : PokemonCard
    {
        public DarkGolbat(Player owner) : base(owner)
        {
            PokemonName = PokemonNames.DarkGolbat;
			EvolvesFrom = PokemonNames.Zubat;
            Stage = 1;
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
