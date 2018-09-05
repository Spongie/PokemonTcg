using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkRapidash : PokemonCard
    {
        public DarkRapidash(Player owner) : base(owner)
        {
            PokemonName = PokemonNames.DarkRapidash;
			EvolvesFrom = PokemonNames.Ponyta;
            Stage = 1;
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
