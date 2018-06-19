using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class RollingTackle : Attack
    {
        public RollingTackle()
        {
            Name = "Rolling Tackle";
            Description = "";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 10;
        }
		
    }
}
