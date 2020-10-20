using System;
using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.EnergyCards;

namespace BaseSet.Attacks
{
    public class HydroPump : Attack
    {
        public HydroPump()
        {
            Name = "Hydro Pump";
            Description = "Does 40 damage plus 10 more damage for each Water energy attached to Blastoise but not used to pay for this attack's Energy cost. Extra Water Energy after the 2nd doesn't count.";
			DamageText = "40";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Water, 3)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            var unusedWaterEnergy = owner.ActivePokemonCard.AttachedEnergy.OfType<WaterEnergy>().Count() - 3;

            return 40 + (10 * Math.Min(unusedWaterEnergy, 2));
        }
    }
}
