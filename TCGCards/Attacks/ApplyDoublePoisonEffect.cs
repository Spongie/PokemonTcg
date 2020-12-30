using CardEditor.Views;
using Entities;
using TCGCards.Core;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.Attacks
{
    public class ApplyDoublePoisonEffect : Attack
    {
        private CoinFlipConditional coinFlipConditional = new CoinFlipConditional();

        [DynamicInput("Condition", InputControl.Dynamic)]
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
            opponent.ActivePokemonCard.ApplyStatusEffect(StatusEffect.Poison, game);
            opponent.ActivePokemonCard.DoublePoison = true;

            base.ProcessEffects(game, owner, opponent);
        }
    }
}
