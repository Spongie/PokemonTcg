using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class HyperFang : Attack
    {
        public HyperFang()
        {
            Name = "Hyper Fang";
            Description = "Flip a coin. If tails, this attack does nothing.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 3)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 50;
        }
		//TODO:
    }
}
