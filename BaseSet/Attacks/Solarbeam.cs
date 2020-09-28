using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Solarbeam : Attack
    {
        public Solarbeam()
        {
            Name = "Solarbeam";
            Description = "";
			DamageText = "60";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Grass, 4)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 60;
        }
		
    }
}
