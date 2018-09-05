using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TeamRocket.Attacks;

namespace TeamRocket.PokemonCards
{
    public class DarkElectrode : PokemonCard
    {
        public DarkElectrode(Player owner) : base(owner)
        {
            PokemonName = "Dark Electrode";
			EvolvesFrom = "Voltorb";
            Stage = 1;
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
