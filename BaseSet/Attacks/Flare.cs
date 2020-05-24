using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Flare : Attack
    {
        public Flare()
        {
            Name = "Flare";
            Description = "";
			DamageText = "20";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fire, 1),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 20;
        }
		
    }
}
