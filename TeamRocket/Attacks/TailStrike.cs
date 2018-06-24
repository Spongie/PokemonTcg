using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class TailStrike : Attack
    {
        public TailStrike()
        {
            Name = "Tail Strike";
            Description = "Flip a coin. If heads, this attack does 20 damage plus 20 more damage. If tails, this attack does 20 damage.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 3)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 20;
        }
		//TODO:
    }
}
