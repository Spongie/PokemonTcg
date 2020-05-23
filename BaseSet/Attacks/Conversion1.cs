using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Conversion1 : Attack
    {
        public Conversion1()
        {
            Name = "Conversion 1";
            Description = "If the Defending Pokémon has a Weakness, you may change it to a type of your choice other than Colorless.";
			DamageText = "0";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }
		//TODO: Special effects
    }
}
