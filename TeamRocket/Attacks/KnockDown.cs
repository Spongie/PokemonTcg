using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class KnockDown : Attack
    {
        public KnockDown()
        {
            Name = "Knock Down";
            Description = "You opponent flips a coin. If tails, this attack does 20 damage plus 20 more damage. If heads, this attack does 20 damage.";
            DamageText = "20+";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fighting, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return CoinFlipper.FlipCoin() ? 20 : 40;
        }
    }
}
