using CardEditor.Views;
using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class FlipCoinPlusAttack : Attack
    {
        private int extraForHeads;
        private int extraForTails;

        public FlipCoinPlusAttack() : base()
        {
            Name = "Flip coin attack";
        }

        [DynamicInput("Extra damage if heads")]
        public int ExtraforHeads
        {
            get { return extraForHeads; }
            set
            {
                extraForHeads = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Extra damage if Tails")]
        public int ExtraforTails
        {
            get { return extraForTails; }
            set
            {
                extraForTails = value;
                FirePropertyChanged();
            }
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            int extraDamage = 0;

            if (game.FlipCoins(1) == 1)
            {
                extraDamage = ExtraforHeads;
            }
            else
            {
                extraDamage = ExtraforTails;
            }

            return extraDamage + Damage;
        }
    }
}
