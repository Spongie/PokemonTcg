using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Dig : Attack
    {
        public Dig()
        {
            Name = "Dig";
            Description = "";
			DamageText = "10";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fighting, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 10;
        }
		
    }
}
