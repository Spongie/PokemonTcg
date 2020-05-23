using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class DoubleKick : Attack
    {
        public DoubleKick()
        {
            Name = "Double Kick";
            Description = "Flip 2 coins. This attack does 30 damage times the number of heads.";
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
		//TODO: Special effects
    }
}
