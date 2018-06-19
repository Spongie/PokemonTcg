using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkGolduck : IPokemonCard
    {
        public DarkGolduck(Player owner) : base(owner)
        {
            PokemonName = "Dark Golduck";
			EvolvesFrom = "Psyduck"; //TODO: Add stage
            Hp = 60;
            PokemonType = EnergyTypes.Water;
            RetreatCost = 2;
            Weakness = EnergyTypes.Electric;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new ThirdEye(),
				new SuperPsy()
            };
			
        }
    }
}
