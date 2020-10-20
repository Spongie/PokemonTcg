using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class FirePunch : Attack
    {
        public FirePunch()
        {
            Name = "Fire Punch";
            Description = "";
			DamageText = "30";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fire, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 30;
        }
		
    }
}
