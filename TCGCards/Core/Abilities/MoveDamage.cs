using CardEditor.Views;
using TCGCards.TrainerEffects;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.Core.Abilities
{
    public class MoveDamage : Ability
    {
        private bool allowKnockout;
        private TargetingMode targetingMode;
        private TargetingMode sourceTarget;
        private int amount;

        public MoveDamage() :this(null)
        {

        }

        public MoveDamage(PokemonCard owner) : base(owner)
        {
            TriggerType = TriggerType.Activation;
        }

        [DynamicInput("Damge to move")]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Source", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode SourceTargetingMode
        {
            get { return sourceTarget; }
            set
            {
                sourceTarget = value;
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

        [DynamicInput("Allow knockout target", InputControl.Boolean)]
        public bool AllowKnockout
        {
            get { return allowKnockout; }
            set
            {
                allowKnockout = value;
                FirePropertyChanged();
            }
        }

        public override bool CanActivate(GameField game, Player caster, Player opponent)
        {
            if (!AllowKnockout && TargetingMode == TargetingMode.Self && PokemonOwner.DamageCounters + amount >= PokemonOwner.Hp)
            {
                return false;
            }

            return base.CanActivate(game, caster, opponent);
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            while (true)
            {
                var source = Targeting.AskForTargetFromTargetingMode(SourceTargetingMode, game, owner, opponent, PokemonOwner, "Select target to move damage from");

                if (source == null)
                {
                    return;
                }

                if (source.DamageCounters < Amount)
                {
                    continue;
                }

                var target = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, owner, opponent, PokemonOwner, "Select target to move damage to");

                if (target == null)
                {
                    return;
                }

                if (!allowKnockout && target.DamageCounters + Amount >= target.Hp)
                {
                    continue;
                }

                source.Heal(Amount, game);
                target.DealDamage(Amount, game, null, false);
                break;
            }
        }
    }
}
