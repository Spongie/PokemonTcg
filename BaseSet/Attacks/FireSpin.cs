using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class FireSpin : Attack
    {
        public FireSpin()
        {
            Name = "Fire Spin";
            Description = "Discard 2 Energy cards attached to Charizard in order to use this attack.";
			DamageText = "100";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fire, 4)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 100;
        }
		//TODO: Special effects
    }
}
