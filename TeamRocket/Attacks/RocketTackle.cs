using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class RocketTackle : Attack
    {
        public RocketTackle()
        {
            Name = "Rocket Tackle";
            Description = "Dark Blastoise does 10 damage to itself. Flip a coin. If heads, prevent all damage done to Dark Blastoise during your opponent&#8217;s next turn. (Any other effects of attacks still happen.)";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Water, 1),
				new Energy(EnergyTypes.Colorless, 2)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 40;
        }
		//TODO:
    }
}
