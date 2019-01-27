using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Kick : Attack
    {
        public Kick()
        {
            Name = "Kick";
            Description = "";
            DamageText = "30";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fighting, 1),
				new Energy(EnergyTypes.Colorless, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 30;
        }
		
    }
}
