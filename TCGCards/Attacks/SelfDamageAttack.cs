using CardEditor.Views;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class SelfDamageAttack : Attack
    {
        private int amount;
        private bool coinFlip;

        public SelfDamageAttack() :base()
        {
            Name = "Self damage attack";
        }

        [DynamicInput("Coin flipped", InputControl.Boolean)]
        public bool CoinFlip
        {
            get { return coinFlip; }
            set
            {
                coinFlip = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("SelfDamage")]
        public int Amount
        {
            get { return amount; }
            set 
            { 
                amount = value;
                FirePropertyChanged();
            }
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (CoinFlip && game.FlipCoins(1) == 0)
            {
                return;
            }

            owner.ActivePokemonCard.DealDamage(amount, game, owner.ActivePokemonCard);
            base.ProcessEffects(game, owner, opponent);
        }
    }
}
