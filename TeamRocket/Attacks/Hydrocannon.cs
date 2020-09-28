using System;
using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Hydrocannon : Attack
    {
        public Hydrocannon()
        {
            Name = "Hydrocannon";
            Description = "Does 30 damage plus 20 more damage for each [W] Energy attached to Dark Blastoise but not used to pay for this attack. You can't add more than 40 damage in this way.";
            DamageText = "30+";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Water, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            int extraDamage = 0;
            int extraWaterEnergy = owner.ActivePokemonCard.GetEnergyOfType(EnergyTypes.Water) - 2;

            extraDamage = Math.Max(40, extraWaterEnergy * 20);

            return 30;
        }
    }
}
