using CardEditor.Views;
using TCGCards.Core;
using TCGCards.Core.Abilities;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.Attacks
{
    public class ApplyAttackFailOnTails : Attack
    {
        private CoinFlipConditional coinFlipConditional = new CoinFlipConditional();

        [DynamicInput("Coin flip", InputControl.Dynamic)]
        public CoinFlipConditional CoinFlipConditional
        {
            get { return coinFlipConditional; }
            set
            {
                coinFlipConditional = value;
                FirePropertyChanged();
            }
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (!CoinFlipConditional.IsOk(game, owner))
            {
                return;
            }

            opponent.ActivePokemonCard.TemporaryAbilities.Add(new AttackStopperSpecificAbility(owner.ActivePokemonCard)
            {
                CoinFlip = true,
                OnlyCurrentTarget = false,
                StopOnTails = true,
                IsBuff = true
            });

            base.ProcessEffects(game, owner, opponent);
        }
    }
}
