using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Twineedle : Attack
    {
        public Twineedle()
        {
            Name = "Twineedle";
            Description = "Flip 2 coins. This attack does 30 damage times the number of heads.";
			DamageText = "30";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 3)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 30;
        }
		//TODO: Special effects
    }
}
