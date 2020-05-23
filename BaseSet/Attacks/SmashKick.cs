using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class SmashKick : Attack
    {
        public SmashKick()
        {
            Name = "Smash Kick";
            Description = "";
			DamageText = "20";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 20;
        }
		
    }
}
