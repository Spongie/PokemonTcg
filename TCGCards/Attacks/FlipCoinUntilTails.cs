using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class FlipCoinUntilTails : Attack
    {
        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            var totalDamage = 0;

            while (game.FlipCoins(1) == 1)
            {
                totalDamage += Damage;
            }

            return totalDamage;
        }
    }
}
