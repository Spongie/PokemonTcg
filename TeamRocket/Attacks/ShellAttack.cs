using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class ShellAttack : Attack
    {
        public ShellAttack()
        {
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 2)
            };
            Name = "Shell attack";
            DamageText = "20";
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 20;
        }
    }
}
