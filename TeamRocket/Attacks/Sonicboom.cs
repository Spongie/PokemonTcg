using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Sonicboom : Attack
    {
        public Sonicboom()
        {
            Name = "Sonicboom";
            Description = "Don&#8217;t apply Weakness and Resistance for this attack. (Any other effects that would happen after applying Weakness and Resistance still happen.)";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 2)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 20;
        }
		//TODO:
    }
}
