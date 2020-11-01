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
            if (CoinFlip)
            {
                if (game.FlipCoins(1) == 0)
                {
                    return;
                }
            }

            if (maxDamage > 0)
            {
                owner.ActivePokemonCard.DamageStoppers.Add(new DamageStopper((x) => x <= maxDamage) { Amount = amount });
            }
            else
            {
                owner.ActivePokemonCard.DamageStoppers.Add(new DamageStopper((x) => true) { Amount = amount });
            }
        }
    }
}
