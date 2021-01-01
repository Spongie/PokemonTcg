using CardEditor.Views;
using Entities;
using Entities.Models;
using TCGCards.Core;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class DamageTimesEnergyEffect : DataModel, IEffect
    {
        private EnergyTypes energyType;
        private int amount;
        private TargetingMode targetingMode;

        public string EffectType
        {
            get
            {
                return "Damage x Energy";
            }
        }

        [DynamicInput("Energy type", InputControl.Dropdown, typeof(EnergyTypes))]
        public EnergyTypes EnergyType
        {
            get { return energyType; }
            set
            {
                energyType = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Damage amount")]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Target?", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode TargetingMode
        {
            get { return targetingMode; }
            set
            {
                targetingMode = value;
                FirePropertyChanged();
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return true;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            int damage = 0;

            foreach (var energy in caster.ActivePokemonCard.AttachedEnergy)
            {
                if (EnergyType == EnergyTypes.All || energy.EnergyType == EnergyType)
                {
                    damage += Amount;
                }
            }

            var target = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, pokemonSource);
            target.DealDamage(damage, game, pokemonSource, true);
        }
    }
}
