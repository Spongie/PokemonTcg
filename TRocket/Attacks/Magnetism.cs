using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Magnetism : Attack
    {
        public Magnetism()
        {
            Name = "Magnetism";
            Description = "Does 10 damage plus 10 more damage for each Magnemite, Magneton, and Dark Magneton on your Bench.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Electric, 1),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 10;
        }
		//TODO:
    }
}
