using System;

namespace TCGCards.Core
{
    public static class DamageCalculator
    {
        public static int GetDamageAfterWeaknessAndResistance(int damage, PokemonCard attacker, PokemonCard defender, Attack attack)
        {
            var realDamage = damage;

            if (attack != null && !attack.ApplyWeaknessResistance)
            {
                return realDamage;
            }

            if (defender.Resistance == attacker.Type)
            {
                realDamage -= 30;
            }
            if (defender.Weakness == attacker.Type)
            {
                realDamage *= 2;
            }

            return Math.Max(realDamage, 0);
        }
    }
}
