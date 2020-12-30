using CardEditor.Views;
using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class FlipCoinPlusAttack : Attack
    {
        private int extraForHeads;
        private int extraForTails;
        private int coinsToFlip = 1;
        private int headsForBonus = 1;
        private bool bonusForAllHeads;

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

        [DynamicInput("Coins to flip")]
        public int CoinsToFlip
        {
            get { return coinsToFlip; }
            set
            {
                coinsToFlip = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Heads required")]
        public int HeadsForBonus
        {
            get { return headsForBonus; }
            set
            {
                headsForBonus = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Bonus for all heads", InputControl.Boolean)]
        public bool BonusForAllHeads
        {
            get { return bonusForAllHeads; }
            set
            {
                bonusForAllHeads = value;
                FirePropertyChanged();
            }
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            int extraDamage;
            var heads = game.FlipCoins(CoinsToFlip);

            if (bonusForAllHeads)
            {
                extraDamage = ExtraforHeads * heads;
            }
            else if (heads >= HeadsForBonus)
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
