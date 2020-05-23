using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Psyshock : Attack
    {
        public Psyshock()
        {
            Name = "Psyshock";
            Description = "Flip a coin. If heads, the Defending Pokémon is now Paralyzed.";
			DamageText = "10";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Psychic, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 10;
        }
		//TODO: Special effects
    }
}
