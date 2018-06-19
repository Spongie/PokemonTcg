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
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fire, 1)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 20;
        }
		
    }
}
