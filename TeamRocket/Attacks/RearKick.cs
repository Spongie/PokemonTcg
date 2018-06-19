using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class RearKick : Attack
    {
        public RearKick()
        {
            Name = "Rear Kick";
            Description = "";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 2)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 20;
        }
		
    }
}
