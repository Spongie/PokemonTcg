using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Harden : Attack
    {
        public Harden()
        {
            Name = "Harden";
            Description = "During opponent's next turn, whenever 30 or less damage is done to Onix (after applying Weakness and Resistance), prevent that damage. (Any other effects of attacks still happen.)";
			DamageText = "0";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fighting, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }
		//TODO: Special effects
    }
}
