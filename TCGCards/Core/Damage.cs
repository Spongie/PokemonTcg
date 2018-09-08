using System;

namespace TCGCards.Core
{
    public class Damage
    {
        public Damage() : this(0,0)
        {

        }

        public Damage(int normalDamage) : this(normalDamage, 0)
        {

        }

        public Damage(int normalDamage, int damageWithoutResistAndWeakness)
        {
            NormalDamage = normalDamage;
            DamageWithoutResistAndWeakness = damageWithoutResistAndWeakness;
        }

        public int NormalDamage { get; set; }
        public int DamageWithoutResistAndWeakness { get; set; }

        public static implicit operator Damage(int value)
        {
            return new Damage(value);
        }

        internal bool IsZero()
        {
            return NormalDamage == 0 && DamageWithoutResistAndWeakness == 0;
        }
    }
}
