using CardEditor.Views;
using Entities;
using Entities.Models;
using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class ApplyStatusEffect : DataModel, IEffect
    {
        private StatusEffect statusEffect;
        private bool flipCoin;
        private TargetingMode targetingMode = TargetingMode.OpponentActive;

        [DynamicInput("Flip Coin?", InputControl.Boolean)]
        public bool FlipCoin
        {
            get { return flipCoin; }
            set
            {
                flipCoin = value;
                FirePropertyChanged();
            }
        }

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

        [DynamicInput("Target", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode TargetingMode
        {
            get { return targetingMode; }
            set
            {
                targetingMode = value;
                FirePropertyChanged();
            }
        }

        public string EffectType
        {
            get
            {
                return "Apply Status";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return true;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand)
        {
            switch (StatusEffect)
            {
                case StatusEffect.Sleep:
                    attachedTo.IsAsleep = true;
                    break;
                case StatusEffect.Poison:
                    attachedTo.IsPoisoned = true;
                    break;
                case StatusEffect.Paralyze:
                    attachedTo.IsParalyzed = true;
                    break;
                case StatusEffect.Burn:
                    attachedTo.IsBurned = true;
                    break;
                case StatusEffect.Confuse:
                    attachedTo.IsConfused = true;
                    break;
                default:
                    break;
            }
        }

        public void Process(GameField game, Player caster, Player opponent)
        {
            if (flipCoin && game.FlipCoins(1) == 0)
            {
                return;
            }

            var target = CardUtil.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent);

            switch (StatusEffect)
            {
                case StatusEffect.Sleep:
                    target.IsAsleep = true;
                    break;
                case StatusEffect.Poison:
                    target.IsPoisoned = true;
                    break;
                case StatusEffect.Paralyze:
                    target.IsParalyzed = true;
                    break;
                case StatusEffect.Burn:
                    target.IsBurned = true;
                    break;
                case StatusEffect.Confuse:
                    target.IsConfused = true;
                    break;
                default:
                    break;
            }
        }
    }
}
