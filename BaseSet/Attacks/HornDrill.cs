using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class HornDrill : Attack
    {
        public HornDrill()
        {
            Name = "Horn Drill";
            Description = "";
			DamageText = "50";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Grass, 2),
				new Energy(EnergyTypes.Colorless, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 50;
        }
		
    }
}
