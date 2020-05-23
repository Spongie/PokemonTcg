using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class VineWhip : Attack
    {
        public VineWhip()
        {
            Name = "Vine Whip";
            Description = "";
			DamageText = "30";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Grass, 1),
				new Energy(EnergyTypes.Colorless, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 30;
        }
		
    }
}
