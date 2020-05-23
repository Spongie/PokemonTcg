using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class LowKick : Attack
    {
        public LowKick()
        {
            Name = "Low Kick";
            Description = "";
			DamageText = "20";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fighting, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 20;
        }
		
    }
}
