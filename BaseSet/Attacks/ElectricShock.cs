using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class ElectricShock : Attack
    {
        public ElectricShock()
        {
            Name = "Electric Shock";
            Description = "Flip a coin. If tails, Electrode does 10 damage to itself.";
			DamageText = "50";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Electric, 3)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 50;
        }
		//TODO: Special effects
    }
}
