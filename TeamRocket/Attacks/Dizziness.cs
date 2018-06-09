﻿using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    public class Dizziness : Attack
    {
        public Dizziness()
        {
            Name = "Dizziness";
            Description = "Draw a card";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Psychic, 1)
            };
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            owner.DrawCards(1);
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 0;
        }
    }
}
