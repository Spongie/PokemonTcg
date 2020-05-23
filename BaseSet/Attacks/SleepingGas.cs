using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class SleepingGas : Attack
    {
        public SleepingGas()
        {
            Name = "Sleeping Gas";
            Description = "Flip a coin. If heads, the Defending Pokémon is now Asleep.";
			DamageText = "0";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Psychic, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }
		//TODO: Special effects
    }
}
