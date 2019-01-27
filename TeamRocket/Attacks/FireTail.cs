using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class FireTail : Attack
    {
        public FireTail()
        {
            Name = "Fire Tail";
            Description = "";
            DamageText = "20";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fire, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 20;
        }
		
    }
}
