using System;
using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class WaterGun : Attack
    {
        public WaterGun()
        {
            Name = "Water Gun";
            Description = "Draw a card";
            DamageText = "20+";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Water, 1),
                new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            int waterEnergyAmount = owner.ActivePokemonCard.AttachedEnergy.Count(energy => energy.EnergyType == EnergyTypes.Water) - 1;

            if (!owner.ActivePokemonCard.AttachedEnergy.Any(energy => energy.EnergyType != EnergyTypes.Water))
            {
                waterEnergyAmount--;
            }

            return 20 + Math.Min(20, waterEnergyAmount * 10);
        }
    }
}
