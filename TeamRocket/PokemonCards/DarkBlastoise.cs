using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkBlastoise : IPokemonCard
    {
        public DarkBlastoise(Player owner) : base(owner)
        {
            PokemonName = "Dark Blastoise";
			EvolvesFrom = "Dark Wartortle"; //TODO: Add stage
            Hp = 70;
            PokemonType = EnergyTypes.Water;
            RetreatCost = 2;
            Weakness = EnergyTypes.Electric;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new Hydrocannon(),
				new RocketTackle()
            };
			
        }
    }
}
