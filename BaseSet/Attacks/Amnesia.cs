using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Amnesia : Attack
    {
        public Amnesia()
        {
            Name = "Amnesia";
            Description = "Choose 1 of defenders attacks. Defender cannot use that attack next turn.";
			DamageText = "0";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Water, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }
		//TODO: Special effects
    }
}
