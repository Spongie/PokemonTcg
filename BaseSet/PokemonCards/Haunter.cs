using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Haunter : PokemonCard
    {
        public Haunter(Player owner) : base(owner)
        {
            PokemonName = "Haunter";
			EvolvesFrom = "//TODO: Evolve";
            Hp = 60;
            PokemonType = EnergyTypes.Psychic;
            RetreatCost = 1;
            Weakness = EnergyTypes.None;
			Resistance = EnergyTypes.Fighting;
			Stage = 1;
            Attacks = new List<Attack>
            {
				new Hypnosis(),
				new DreamEater()
            };
			
        }
    }
}
