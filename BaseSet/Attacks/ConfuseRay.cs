using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class ConfuseRay : Attack
    {
        public ConfuseRay()
        {
            Name = "Confuse Ray";
            Description = "Flip a coin. If heads, the Defending Pokémon is now Confused.";
			DamageText = "10";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fighting, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 10;
        }
		//TODO: Special effects
    }
}
