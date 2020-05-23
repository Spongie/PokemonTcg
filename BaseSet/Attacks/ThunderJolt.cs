using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class ThunderJolt : Attack
    {
        public ThunderJolt()
        {
            Name = "Thunder Jolt";
            Description = "Flip a coin. If tails, Pikachu does 10 damage to itself.";
			DamageText = "30";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Electric, 1),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 30;
        }
		//TODO: Special effects
    }
}
