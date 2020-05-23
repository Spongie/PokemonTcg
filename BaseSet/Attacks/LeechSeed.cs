using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class LeechSeed : Attack
    {
        public LeechSeed()
        {
            Name = "Leech Seed";
            Description = "Unless all damage from this attack is prevented, you may remove 1 damage counter from Bulbasaur.";
			DamageText = "20";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Grass, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 20;
        }
		//TODO: Special effects
    }
}
