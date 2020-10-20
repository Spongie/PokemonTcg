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
            switch (StatusEffect)
            {
                case StatusEffect.Sleep:
                    opponent.ActivePokemonCard.IsAsleep = true;
                    break;
                case StatusEffect.Poison:
                    opponent.ActivePokemonCard.IsPoisoned = true;
                    break;
                case StatusEffect.Paralyze:
                    opponent.ActivePokemonCard.IsParalyzed = true;
                    break;
                case StatusEffect.Burn:
                    opponent.ActivePokemonCard.IsBurned = true;
                    break;
                case StatusEffect.Confuse:
                    opponent.ActivePokemonCard.IsConfused = true;
                    break;
                default:
                    break;
            }
        }
    }
}
