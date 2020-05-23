using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Recover : Attack
    {
        public Recover()
        {
            Name = "Recover";
            Description = "Discard 1 Energy card attached to Kadabra in order use this attack. Remove all damage counters from Kadabra.";
			DamageText = "0";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Psychic, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }
		//TODO: Special effects
    }
}
