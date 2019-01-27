﻿using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Bite20 : Attack
    {
        public Bite20()
        {
            Name = "Bite";
            Description = string.Empty;
            DamageText = "20";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 1),
                new Energy(EnergyTypes.Grass, 1)
            };
        }


        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 20;
        }
    }
}
