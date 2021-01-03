using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class ESP : Metronome
    {
        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            int heads = game.FlipCoins(3);

            if (heads == 1)
            {
                owner.DrawCards(1);
            }
            else if (heads == 2)
            {
                return 20;
            }
            else if (heads == 3)
            {
                return base.GetDamage(owner, opponent, game);
            }

            return 0;
        }
    }
}
