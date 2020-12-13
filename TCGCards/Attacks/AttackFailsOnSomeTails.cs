using CardEditor.Views;
using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class AttackFailsOnSomeTails : Attack
    {
        private int coins;
        private int tailsToFail;

        [DynamicInput("Tails before fail")]
        public int TailsToFail
        {
            get { return tailsToFail; }
            set
            {
                tailsToFail = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Coins to flip")]
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
            int tails = Coins - game.FlipCoins(Coins);

            if (tails >= TailsToFail)
            {
                game?.GameLog.AddMessage("The attack did nothing");
                return 0;
            }

            return base.GetDamage(owner, opponent, game);
        }
    }
}
