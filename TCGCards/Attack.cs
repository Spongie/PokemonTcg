using System;
using System.Collections.Generic;
using TCGCards.Core;

namespace TCGCards
{
    public abstract class Attack
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public List<Energy> Cost { get; set; }

        public bool NeedsPlayerInteraction { get; protected set; }

        public abstract int GetDamage(Player owner, Player opponent);

        public virtual void ProcessEffects(GameField game, Player owner, Player opponent) { }

        protected int GetDamageAfterResistanceAndWeakness(int baseDamage, IPokemonCard attacker, IPokemonCard target)
        {
            int realDamage = baseDamage;

            if (target.Resistance == attacker.PokemonType)
            {
                realDamage -= 30;
            }
            if (target.Weakness == attacker.PokemonType)
            {
                realDamage *= 2;
            }

            return Math.Max(realDamage, 0);
        }
    }
}