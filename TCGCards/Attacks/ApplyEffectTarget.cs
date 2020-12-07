using CardEditor.Views;
using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class ApplyEffectTarget : Attack
    {
        public ApplyEffectTarget() : base()
        {
            Name = "Apply Effect Attack";
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
            opponent.ActivePokemonCard.ApplyStatusEffect(StatusEffect, game);

            base.ProcessEffects(game, owner, opponent);
        }
    }
}
