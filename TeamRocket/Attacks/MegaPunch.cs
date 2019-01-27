using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class MegaPunch : Attack
    {
        public MegaPunch()
        {
            Name = "Mega Punch";
            Description = "";
            DamageText = "30";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fighting, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 30;
        }
		
    }
}
