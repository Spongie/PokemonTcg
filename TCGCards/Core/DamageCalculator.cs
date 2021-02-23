using System;
using TCGCards.Core.Abilities;

namespace TCGCards.Core
{
    public static class DamageCalculator
    {
        public static int GetDamageAfterWeaknessAndResistance(int damage, PokemonCard attacker, PokemonCard defender, Attack attack, IResistanceModifier resistanceModifier)
        {
            var realDamage = damage;

            if (attack == null)
            {
                return realDamage;
            }

            if (attack.ApplyResistance && defender.Resistance == attacker.Type)
            {
                if (resistanceModifier != null)
                {
                    realDamage -= resistanceModifier.GetModifiedResistance(attacker, defender);
                }
                else
                {
                    realDamage -= defender.ResistanceAmount;
                }
            }
            if (attack.ApplyWeakness &&  defender.Weakness == attacker.Type)
            {
                realDamage *= 2;
            }

            return Math.Max(realDamage, 0);
        }
    }
}
