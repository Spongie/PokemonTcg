using CardEditor.Views;
using TCGCards.Core;
using TCGCards.Core.SpecialAbilities;

namespace TCGCards.Attacks
{
    public class ApplyDamagePreventionEffect : Attack
    {
        private int amount;
        private bool coinFlip;
        private int maxDamage;
        private bool onlyProtectSelf = true;

        [DynamicInput("Only protect self", InputControl.Boolean)]
        public bool OnlyProtectSelf
        {
            get { return onlyProtectSelf; }
            set
            {
                onlyProtectSelf = value;
                FirePropertyChanged();
            }
        }


        [DynamicInput("Prevention Limit")]
        public int MaxDamage
        {
            get { return maxDamage; }
            set 
            { 
                maxDamage = value;
                FirePropertyChanged();
            }
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

        [DynamicInput("Damage to prevent")]
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

            DamageStopper damageStopper;

            if (maxDamage > 0)
            {
                damageStopper = new DamageStopper((x) => x <= maxDamage) { Amount = amount };
            }
            else
            {
                damageStopper = new DamageStopper((x) => true) { Amount = amount };
            }

            if (onlyProtectSelf)
            {
                owner.ActivePokemonCard.DamageStoppers.Add(damageStopper);
            }
            else
            {
                game.DamageStoppers.Add(damageStopper);
            }

            base.ProcessEffects(game, owner, opponent);
        }
    }
}
