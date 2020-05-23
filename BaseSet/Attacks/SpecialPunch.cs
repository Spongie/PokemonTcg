using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class SpecialPunch : Attack
    {
        public SpecialPunch()
        {
            Name = "Special Punch";
            Description = "";
			DamageText = "40";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fighting, 2),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 40;
        }
		
    }
}
