using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class GiantTail : Attack
    {
        public GiantTail()
        {
            Name = "Giant Tail";
            Description = "Flip a coin. If tails, this attack does nothing.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 4)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 70;
        }
		//TODO:
    }
}
