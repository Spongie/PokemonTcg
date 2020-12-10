using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class FlipCoinUntilTails : Attack
    {
        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return Damage * game.FlipCoinsUntilTails();
        }
    }
}
