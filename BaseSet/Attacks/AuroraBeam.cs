using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class AuroraBeam : Attack
    {
        public AuroraBeam()
        {
            Name = "Aurora Beam";
            Description = "";
			DamageText = "50";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Water, 2),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 50;
        }
		
    }
}
