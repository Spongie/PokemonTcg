using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class QuickAttack : Attack
    {
        public QuickAttack()
        {
            Name = "Quick Attack";
            Description = "Flip a coin. If heads this attack does 10 damage plus 10 more damage; if tails, this attack does 10 damage.";
            DamageText = "10+";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 10 + (CoinFlipper.FlipCoin() ? 10 : 0);
        }
    }
}
