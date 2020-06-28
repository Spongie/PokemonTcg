using System;
using System.Collections.Generic;
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
            var unusedWaterEnergy = owner.ActivePokemonCard.AttachedEnergy.Count - 3;

            return baseDamage + (10 * Math.Max(0, Math.Min(unusedWaterEnergy, 2)));
        }
    }
}
