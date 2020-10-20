using CardEditor.Views;
using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class FlipCoinAttack : Attack
    {
        private int coins;

        public FlipCoinAttack() : base()
        {
            Name = "Flip coin attack";
        }

        [DynamicInput("Number of coins")]
        public int Coins
        {
            get { return coins; }
            set
            {
                coins = value;
                FirePropertyChanged();
            }
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            int heads = game.FlipCoins(Coins);

            return heads * Damage;
        }
    }
}
