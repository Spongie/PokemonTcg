using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Conversion2 : Attack
    {
        public Conversion2()
        {
            Name = "Conversion 2";
            Description = "Change Porygon's Resistance to a type of your choice other than Colorless.";
			DamageText = "0";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }
		//TODO: Special effects
    }
}
