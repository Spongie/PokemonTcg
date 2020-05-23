using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Flamethrower : Attack
    {
        public Flamethrower()
        {
            Name = "Flamethrower";
            Description = "Discard 1 Energy card attached to Magmar in order to use this attack.";
			DamageText = "50";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fighting, 2),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 50;
        }
		//TODO: Special effects
    }
}
