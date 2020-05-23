using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class DragonRage : Attack
    {
        public DragonRage()
        {
            Name = "Dragon Rage";
            Description = "";
			DamageText = "50";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Water, 3)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 50;
        }
		
    }
}
