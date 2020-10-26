using CardEditor.Views;
using Entities;

namespace TCGCards.Core.Abilities
{
    public class EnergyTypeOverrideTemporaryAbility : TemporaryAbility
    {
        private EnergyTypes sourceType;
        private EnergyTypes newType;

        public EnergyTypeOverrideTemporaryAbility() :base(null)
        {

        }

        public EnergyTypeOverrideTemporaryAbility(PokemonCard owner) : base(owner)
        {

        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameLog log)
        {
            
        }

        [DynamicInput("Old Type", InputControl.Dropdown, typeof(EnergyTypes))]
        public EnergyTypes SourceType
        {
            get { return sourceType; }
            set
            {
                sourceType = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("New Type", InputControl.Dropdown, typeof(EnergyTypes))]
        public EnergyTypes NewType
        {
            get { return newType; }
            set
            {
                newType = value;
                FirePropertyChanged();
            }
        }

        public bool CoversType(EnergyTypes type)
        {
            return SourceType == EnergyTypes.All || SourceType == type;
        }
    }
}
