using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class RockThrow : Attack
    {
        public RockThrow()
        {
            Name = "Rock Throw";
            Description = "";
			DamageText = "10";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fighting, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 10;
        }
		
    }
}
