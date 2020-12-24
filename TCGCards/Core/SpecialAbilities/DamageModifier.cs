using System;

namespace TCGCards.Core.SpecialAbilities
{
    public class DamageModifier
    {
        public DamageModifier()
        {

        }

        public DamageModifier(int newDamage, int turns)
        {
            NewDamage = newDamage;
            TurnsLeft = turns;
        }

        public int NewDamage { get; set; }
        public int TurnsLeft { get; set; }

        public void ReduceTurnCount()
        {
            TurnsLeft--;
        }
    }
}
