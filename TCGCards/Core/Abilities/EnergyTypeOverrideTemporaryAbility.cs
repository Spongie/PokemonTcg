﻿using CardEditor.Views;
using Entities;

namespace TCGCards.Core.Abilities
{
    public class EnergyTypeOverrideTemporaryAbility : Ability
    {
        private EnergyTypes sourceType;
        private EnergyTypes newType;

        public EnergyTypeOverrideTemporaryAbility() : this(null)
        {

        }

        public EnergyTypeOverrideTemporaryAbility(PokemonCard owner) : base(owner)
        {
            TriggerType = TriggerType.Passive;
            TurnDuration = UNTIL_YOUR_NEXT_TURN;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
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
