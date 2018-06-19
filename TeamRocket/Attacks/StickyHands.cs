using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class StickyHands : Attack
    {
        public StickyHands()
        {
            Name = "Sticky Hands";
            Description = "Flip a coin. If heads, this attack does 10 damage plus 20 more damage and the Defending Pokémon is now Paralyzed. If tails, this attack does 10 damage.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Grass, 2)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 10;
        }
		//TODO:
    }
}
