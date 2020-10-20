using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Bite : Attack
    {
        public Bite()
        {
            Name = "Bite";
            Description = "";
			DamageText = "40";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Water, 1),
				new Energy(EnergyTypes.Colorless, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 40;
        }
		
    }
}
