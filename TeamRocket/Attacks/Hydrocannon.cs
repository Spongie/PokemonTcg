using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Hydrocannon : Attack
    {
        public Hydrocannon()
        {
            Name = "Hydrocannon";
            Description = "Does 30 damage plus 20 more damage for each [W] Energy attached to Dark Blastoise but not used to pay for this attack. You can&#8217;t add more than 40 damage in this way.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Water, 2)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 30;
        }
		//TODO:
    }
}
