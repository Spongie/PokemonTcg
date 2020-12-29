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
        private bool discardUnusedEnergy;

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

        [DynamicInput("Discard unused energy", InputControl.Boolean)]
        public bool DiscardUnusedEnergy
        {
            get { return discardUnusedEnergy; }
            set
            {
                discardUnusedEnergy = value;
                FirePropertyChanged();
            }
        }


        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            int energyOfType = owner.ActivePokemonCard.AttachedEnergy.Where(e => e.EnergyType == EnergyType).Sum(e => e.Amount);
            int colorless = owner.ActivePokemonCard.AttachedEnergy.Where(e => e.EnergyType != EnergyType).Sum(e => e.Amount);

            energyOfType -= Cost.Where(c => c.EnergyType == EnergyType).Sum(c => c.Amount);
            colorless -= Cost.Where(c => c.EnergyType != EnergyType).Sum(c => c.Amount);

            if (colorless < 0)
            {
                energyOfType -= -colorless;
            }

            if (DiscardUnusedEnergy)
            {
                int extraEnergy = energyOfType;

                while (extraEnergy > 0)
                {
                    var energy = owner.ActivePokemonCard.AttachedEnergy.FirstOrDefault(x => x.EnergyType == EnergyType);
                    owner.ActivePokemonCard.DiscardEnergyCard(energy, game);
                    extraEnergy -= energy.Amount;
                }
            }

            return Damage + Math.Max(Math.Min(MaxExtraDamage, energyOfType * AmountPerEnergy), 0);
        }
    }
}
