using CardEditor.Views;
using Entities;
using TCGCards.Core;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.Attacks
{
    public class ExtraDamageIfStatus : Attack
    {
        private StatusEffect statusEffect;
        private TargetingMode targetingMode;
        private int extra;
        private bool applySameStatus;

        [DynamicInput("Required status", InputControl.Dropdown, typeof(StatusEffect))]
        public StatusEffect StatusEffect
        {
            get { return statusEffect; }
            set
            {
                statusEffect = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Target to check", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode TargetingMode
        {
            get { return targetingMode; }
            set
            {
                targetingMode = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Extra damage")]
        public int Extra
        {
            get { return extra; }
            set
            {
                extra = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Apply same status", InputControl.Boolean)]
        public bool ApplySameStatus
        {
            get { return applySameStatus; }
            set
            {
                applySameStatus = value;
                FirePropertyChanged();
            }
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            var target = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, owner, opponent, owner.ActivePokemonCard);

            int extra = 0;

            if (target.HaveStatus(StatusEffect))
            {
                extra = Extra;

                if (applySameStatus)
                {
                    opponent.ActivePokemonCard.ApplyStatusEffect(StatusEffect, game);
                }
            }

            return Damage + extra;
        }
    }
}
