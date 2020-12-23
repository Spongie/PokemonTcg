using CardEditor.Views;
using TCGCards.Core;
using TCGCards.Core.Abilities;

namespace TCGCards.Attacks
{
    //prevent all effects of attacks, including damage, done to self
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

            var source = owner.ActivePokemonCard;

            source.TemporaryAbilities.Add(new DamageTakenModifier(source)
            {
                Modifer = 999999,
                IsBuff = true
            });
            source.TemporaryAbilities.Add(new PreventStatusEffects(source)
            {
                IsBuff = true,
                PreventBurn = true,
                PreventConfuse = true,
                PreventParalyze = true,
                PreventPoison = true,
                PreventSleep = true
            });

            base.ProcessEffects(game, owner, opponent);
        }
    }
}
