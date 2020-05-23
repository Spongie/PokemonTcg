using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class FuryAttack : Attack
    {
        public FuryAttack()
        {
            Name = "Fury Attack";
            Description = "Flip 2 coins. This attack does 10 damage times the number of heads.";
			DamageText = "10";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 10;
        }
		//TODO: Special effects
    }
}
