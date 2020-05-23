using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class WaterGun : Attack
    {
        public WaterGun()
        {
            Name = "Water Gun";
            Description = "Does 10 damage plus 10 damage for each Energy attached to Poliwag but not used to pay for this attack's Energy cost. Extra Energy after the end don't count.";
			DamageText = "10";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Water, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 10;
        }
		//TODO: Special effects
    }
}
