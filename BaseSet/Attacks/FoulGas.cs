using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class FoulGas : Attack
    {
        public FoulGas()
        {
            Name = "Foul Gas";
            Description = "Flip a coin. If heads, the Defending Pokémon is now Poisoned; if tails, it is now Confused.";
			DamageText = "10";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Grass, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 10;
        }
		//TODO: Special effects
    }
}
