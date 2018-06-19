using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkJolteon : IPokemonCard
    {
        public DarkJolteon(Player owner) : base(owner)
        {
            PokemonName = "Dark Jolteon";
			EvolvesFrom = "Eevee"; //TODO: Add stage
            Hp = 50;
            PokemonType = EnergyTypes.Electric;
            RetreatCost = 1;
            Weakness = EnergyTypes.Fighting;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new LightningFlash(),
				new ThunderAttack()
            };
			
        }
    }
}
