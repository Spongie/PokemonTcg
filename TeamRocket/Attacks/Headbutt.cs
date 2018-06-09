﻿using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    public class Headbutt : Attack
    {
        public Headbutt()
        {
            Name = "Headbutt";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Psychic, 1)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 10;
        }
    }
}