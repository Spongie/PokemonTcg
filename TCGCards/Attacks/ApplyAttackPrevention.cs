using CardEditor.Views;
using TCGCards.Core;
using TCGCards.Core.SpecialAbilities;

namespace TCGCards.Attacks
{
    public class ApplyAttackPrevention : Attack
    {
        private bool coinFlip;
        private bool onlySelf = true;

        [DynamicInput("Only prevent on self", InputControl.Boolean)]
        public bool OnlySelf
        {
            get { return onlySelf; }
            set
            {
                onlySelf = value;
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

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (CoinFlip && game.FlipCoins(1) == 0)
            {
                return;
            }

            if (onlySelf)
            {
                owner.ActivePokemonCard.AttackStoppers.Add(new AttackStopper((x) => x.Equals(owner.ActivePokemonCard)));
            }
            else
            {
                owner.ActivePokemonCard.AttackStoppers.Add(new AttackStopper((x) => true));
            }

            base.ProcessEffects(game, owner, opponent);
        }
    }
}
