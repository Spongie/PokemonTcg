using CardEditor.Views;
using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class FlipCoinApplyEffectTarget : Attack
    {
        public FlipCoinApplyEffectTarget() : base()
        {
            Name = "Flip coin effect";
        }

        private StatusEffect statusEffect;

        [DynamicInput("Status Effect", InputControl.Dropdown, typeof(StatusEffect))]
        public StatusEffect StatusEffect
        {
            get { return statusEffect; }
            set 
            { 
                statusEffect = value;
                FirePropertyChanged();
            }
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (game.FlipCoins(1) > 0)
            {
                opponent.ActivePokemonCard.ApplyStatusEffect(StatusEffect);
            }
        }
    }
}
