using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Jab : Attack
    {
        public Jab()
        {
            Name = "Jab";
            Description = "";
			DamageText = "20";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fighting, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 20;
        }
		
    }
}
