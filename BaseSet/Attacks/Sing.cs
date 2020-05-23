using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Sing : Attack
    {
        public Sing()
        {
            Name = "Sing";
            Description = "Flip a coin. If heads, the Defending Pokémon is now Asleep.";
			DamageText = "0";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }
		//TODO: Special effects
    }
}
