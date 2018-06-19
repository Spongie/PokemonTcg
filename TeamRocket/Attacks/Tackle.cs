using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Tackle : Attack
    {
        public Tackle()
        {
            Name = "Tackle";
            Description = "";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 2)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 20;
        }
		
    }
}
