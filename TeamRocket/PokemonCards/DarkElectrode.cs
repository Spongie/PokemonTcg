using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkElectrode : IPokemonCard
    {
        public DarkElectrode(Player owner) : base(owner)
        {
            PokemonName = "Dark Electrode";
			EvolvesFrom = "Voltorb"; //TODO: Add stage
            Hp = 60;
            PokemonType = EnergyTypes.Electric;
            RetreatCost = 1;
            Weakness = EnergyTypes.Fighting;
			Resistance = EnergyTypes.None;
            Attacks = new List<Attack>
            {
				new RollingTackle(),
				new EnergyBomb()
            };
			
        }
    }
}
