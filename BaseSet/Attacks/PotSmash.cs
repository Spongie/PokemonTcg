using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class PotSmash : Attack
    {
        public PotSmash()
        {
            Name = "Pot Smash";
            Description = "";
			DamageText = "30";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 3)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 30;
        }
		
    }
}
