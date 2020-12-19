using CardEditor.Views;
using Entities;
using Entities.Models;
using System.Linq;
using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class AttachEnergyCardFromDiscard : DataModel, IEffect
    {
        private TargetingMode targetingMode;
        private EnergyTypes energyType = EnergyTypes.All;
        private bool canUseWithoutTarget;

        public string EffectType
        {
            get
            {
                return "Attach energy from discard";
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

        [DynamicInput("Energytype", InputControl.Dropdown, typeof(EnergyTypes))]
        public EnergyTypes EnergyType
        {
            get { return energyType; }
            set
            {
                energyType = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Use without target?", InputControl.Boolean)]
        public bool CanUseWithoutTarget
        {
            get { return canUseWithoutTarget; }
            set
            {
                canUseWithoutTarget = value;
                FirePropertyChanged();
            }
        }


        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return caster.DiscardPile.OfType<EnergyCard>().Count(x => EnergyType == EnergyTypes.All || x.EnergyType == EnergyType) > 1;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            
        }
    }
}
