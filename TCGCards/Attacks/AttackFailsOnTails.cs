using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class AttackFailsOnTails : Attack
    {
        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            if (game.FlipCoins(1) == 0)
            {
                game.GameLog.AddMessage("The attack did nothing");
                return 0;
            }

            return base.GetDamage(owner, opponent, game);
        }
    }
}
