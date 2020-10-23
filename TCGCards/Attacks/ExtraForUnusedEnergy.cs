using CardEditor.Views;
using Entities;
using System;
using System.Linq;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class ExtraForUnusedEnergy : Attack
    {
        private EnergyTypes energyType;
        private int amountPerEnergy;
        private int maxExtraDamage;

        [DynamicInput("Extra Damage per energy")]
        public int AmountPerEnergy
        {
            get { return amountPerEnergy; }
            set 
            { 
                amountPerEnergy = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Maximum Extra Damage")]
        public int MaxExtraDamage
        {
            get { return maxExtraDamage; }
            set 
            { 
                maxExtraDamage = value;
                FirePropertyChanged();
            }
        }


        [DynamicInput("Energy type to count", InputControl.Dropdown, typeof(EnergyTypes))]
        public EnergyTypes EnergyType
        {
            get { return energyType; }
            set 
            { 
                energyType = value;
                FirePropertyChanged();
            }
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            int energyAmount = owner.ActivePokemonCard.AttachedEnergy.Count(energy => energy.EnergyType == EnergyType) - Cost.Sum(x => x.Amount);

            if (!owner.ActivePokemonCard.AttachedEnergy.Any(energy => energy.EnergyType != EnergyType))
            {
                energyAmount--;
            }

            return Damage + Math.Min(MaxExtraDamage, energyAmount * AmountPerEnergy);
        }
    }
}
