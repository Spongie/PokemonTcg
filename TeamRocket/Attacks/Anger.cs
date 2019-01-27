using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Anger : Attack
    {
        public Anger()
        {
            Name = "Anger";
            Description = "Flip a coin. If heads, this attack does 20 damage plus 20 more damage. If tails, this attack does 20 damage.";
            DamageText = "20+";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fighting, 1),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            int extraDamage = CoinFlipper.FlipCoin() ? 20 : 0;
            return 20 + extraDamage;
        }
    }
}
