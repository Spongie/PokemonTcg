using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Doubleedge : Attack
    {
        public Doubleedge()
        {
            Name = "Double-edge";
            Description = "Chansey does 80 damage to itself.";
			DamageText = "80";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 4)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 80;
        }
		//TODO: Special effects
    }
}
