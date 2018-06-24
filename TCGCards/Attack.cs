using System;
using System.Collections.Generic;
using TCGCards.Core;

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
    }
}