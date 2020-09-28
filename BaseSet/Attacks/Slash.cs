using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Slash : Attack
    {
        public Slash()
        {
            Name = "Slash";
            Description = "";
			DamageText = "30";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 3)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 30;
        }
		
    }
}
