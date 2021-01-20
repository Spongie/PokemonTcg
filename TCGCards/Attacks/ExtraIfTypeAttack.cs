using CardEditor.Views;
using Entities;
using TCGCards.Core;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.Attacks
{
    public class ExtraIfTypeAttack : Attack
    {
        private TargetingMode targetingMode = TargetingMode.OpponentActive;
        private EnergyTypes energyTypes;
        private int extraDamage;

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

        [DynamicInput("Type", InputControl.Dropdown, typeof(EnergyTypes))]
        public EnergyTypes TargetType
        {
            get { return energyTypes; }
            set
            {
                energyTypes = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Extra Damage")]
        public int ExtraDamage
        {
            get { return extraDamage; }
            set
            {
                extraDamage = value;
                FirePropertyChanged();
            }
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            var target = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, owner, opponent, owner.ActivePokemonCard);

            if (target.Type == TargetType)
            {
                return Damage + ExtraDamage;
            }

            return base.GetDamage(owner, opponent, game);
        }
    }
}
