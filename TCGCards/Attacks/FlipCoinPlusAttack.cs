using CardEditor.Views;
using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class FlipCoinPlusAttack : Attack
    {
        private int extraForHeads;

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

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            int extraDamage = ExtraforHeads * game.FlipCoins(1);

            return extraDamage + Damage;
        }
    }
}
