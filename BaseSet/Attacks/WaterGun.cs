using System;
using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class WaterGun : Attack
    {
        private int baseDamage;

        public WaterGun(int baseDamage)
        {
            this.baseDamage = baseDamage;
            Name = "Water Gun";
            Description = "Does 10 damage plus 10 damage for each Energy attached to Poliwag but not used to pay for this attack's Energy cost. Extra Energy after the 2nd don't count.";
			DamageText = "10";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Water, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            int waterEnergyAmount = owner.ActivePokemonCard.AttachedEnergy.Count(energy => energy.EnergyType == EnergyTypes.Water) - 1;

            if (!owner.ActivePokemonCard.AttachedEnergy.Any(energy => energy.EnergyType != EnergyTypes.Water))
            {
                waterEnergyAmount--;
            }

            return 10 + Math.Min(10, waterEnergyAmount * 10);
        }
    }
}
