using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Slap : Attack
    {
        public Slap()
        {
            Name = "Slap";
            Description = "";
			DamageText = "20";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Water, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 20;
        }
		
    }
}
