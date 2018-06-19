using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkRapidash : IPokemonCard
    {
        public DarkRapidash(Player owner) : base(owner)
        {
            PokemonName = "Dark Rapidash";
			EvolvesFrom = "Ponyta"; //TODO: Add stage
            Hp = 60;
            PokemonType = EnergyTypes.Fire;
            RetreatCost = 0;
            Weakness = EnergyTypes.Water;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new RearKick(),
				new FlamePillar()
            };
			
        }
    }
}
