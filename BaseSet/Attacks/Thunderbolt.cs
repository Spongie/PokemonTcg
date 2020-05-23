using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Thunderbolt : Attack
    {
        public Thunderbolt()
        {
            Name = "Thunderbolt";
            Description = "Discard all Energy cards attached to Zapdos in order to use this attack.";
			DamageText = "100";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Electric, 4)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 100;
        }
		//TODO: Special effects
    }
}
