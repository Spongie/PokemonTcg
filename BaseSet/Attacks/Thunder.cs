using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Thunder : Attack
    {
        public Thunder()
        {
            Name = "Thunder";
            Description = "Flip a coin. If tails, Zapdos does 30 damage to itself.";
			DamageText = "60";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Electric, 3),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 60;
        }
		//TODO: Special effects
    }
}
