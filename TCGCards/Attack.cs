using System.Collections.Generic;
using TCGCards.Core;
using System.Linq;

namespace TCGCards
{
    public abstract class Attack
    {
        public Attack()
        {
            Description = string.Empty;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<Energy> Cost { get; set; }

        public bool ApplyWeaknessAndResistance { get; set; } = true;

        public abstract Damage GetDamage(Player owner, Player opponent);

        public virtual void ProcessEffects(GameField game, Player owner, Player opponent) { }

        public virtual bool CanBeUsed(GameField game, Player owner, Player opponent)
        {
            var availableEnergy = new List<IEnergyCard>(owner.ActivePokemonCard.AttachedEnergy).OrderBy(card => card.IsBasic).ToList();

            foreach (var energy in Cost.OrderByDescending(cost => cost.EnergyType != EnergyTypes.Colorless))
            {
                for (int i = 0; i < energy.Amount; i+=0)
                {
                    IEnergyCard energyCard = energy.EnergyType == EnergyTypes.Colorless ?
                        availableEnergy.FirstOrDefault()
                        : availableEnergy.FirstOrDefault(card => card.EnergyType == energy.EnergyType);

                    if (energyCard == null)
                    {                 
                        return false;
                    }

                    availableEnergy.Remove(energyCard);

                    i += energyCard.GetEnergry().Amount;
                }
            }

            return true;
        }
    }
}