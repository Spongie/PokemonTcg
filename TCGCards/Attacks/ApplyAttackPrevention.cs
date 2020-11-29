using CardEditor.Views;
using TCGCards.Core;
using TCGCards.Core.Abilities;

namespace TCGCards.Attacks
{
    public class ApplyAttackPrevention : Attack
    {
        private bool coinFlip;
        private bool onlySelf = true;
        private bool onlyCurrentTarget;

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

        [DynamicInput("Only prevent from current target", InputControl.Boolean)]
        public bool OnlyCurrentTarget
        {
            get { return onlyCurrentTarget; }
            set
            {
                onlyCurrentTarget = value;
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

            if (onlySelf || onlyCurrentTarget)
            {
                owner.ActivePokemonCard.TemporaryAbilities.Add(new AttackStopperSpecificAbility(owner.ActivePokemonCard)
                {
                    OnlyCurrentTarget = onlyCurrentTarget,
                    OnlySelf = onlySelf,
                    CurrentTarget = opponent.ActivePokemonCard
                });
            }
            else
            {
                owner.ActivePokemonCard.TemporaryAbilities.Add(new AttackStopperSpecificAbility(owner.ActivePokemonCard)
                {
                    CoinFlip = false,
                    OnlyCurrentTarget = false,
                    OnlySelf = false
                });
            }

            base.ProcessEffects(game, owner, opponent);
        }
    }
}
