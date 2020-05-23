using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class ThunderWave : Attack
    {
        public ThunderWave()
        {
            Name = "Thunder Wave";
            Description = "Flip a coin. If heads, the Defending Pokémon is now Paralyzed.";
			DamageText = "10";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Electric, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 10;
        }
		//TODO: Special effects
    }
}
