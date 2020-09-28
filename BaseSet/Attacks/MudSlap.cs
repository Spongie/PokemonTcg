using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class MudSlap : Attack
    {
        public MudSlap()
        {
            Name = "Mud Slap";
            Description = "";
			DamageText = "30";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fighting, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 30;
        }
		
    }
}
