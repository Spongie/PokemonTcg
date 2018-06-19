using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Scratch : Attack
    {
        public Scratch()
        {
            Name = "Scratch";
            Description = "";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fighting, 1),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 20;
        }
		
    }
}
