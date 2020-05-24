using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using BaseSet.Attacks;

namespace BaseSet.PokemonCards
{
    public class Machoke : PokemonCard
    {
        public Machoke(Player owner) : base(owner)
        {
            PokemonName = "Machoke";
			EvolvesFrom = PokemonNames.Machop;
            Hp = 80;
            PokemonType = EnergyTypes.Fighting;
            RetreatCost = 3;
            Weakness = EnergyTypes.Psychic;
			Resistance = EnergyTypes.None;
			Stage = 1;
            Attacks = new List<Attack>
            {
				new KarateChop(),
				new Submission()
            };
			
        }
    }
}
