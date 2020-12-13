using System;

namespace TCGCards.Core
{
    public static class DamageCalculator
    {
        public static int GetDamageAfterWeaknessAndResistance(int damage, PokemonCard attacker, PokemonCard defender, Attack attack)
        {
            var realDamage = damage;

            if (attack != null)
            {
                return realDamage;
            }

            if (attack.ApplyResistance && defender.Resistance == attacker.Type)
            {
                realDamage -= 30;
            }
            if (attack.ApplyWeakness &&  defender.Weakness == attacker.Type)
            {
                realDamage *= 2;
            }

            return Math.Max(realDamage, 0);
        }
    }
}
