using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class PetalWhirlwind : Attack
    {
        public PetalWhirlwind()
        {
            Name = "Petal Whirlwind";
            Description = "30× damage. Flip a coins. This attack does 30 damage times the number of heads. If you get 2 or more heads, Dark Vileplume is now Confused (after doing damage).";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Grass, 3)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 0;
        }
		//TODO:
    }
}
