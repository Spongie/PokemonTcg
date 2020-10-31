using CardEditor.Views;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class ApplyStatusEffect : DataModel, IEffect
    {
        private StatusEffect statusEffect;
        private bool flipCoin;
        private TargetingMode targetingMode;

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
