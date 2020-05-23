using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Whirlpool : Attack
    {
        public Whirlpool()
        {
            Name = "Whirlpool";
            Description = "If the Defending Pokémon has any Energy cards attached to it, choose 1 and discard it.";
			DamageText = "40";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Water, 2),
				new Energy(EnergyTypes.Colorless, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 40;
        }
		//TODO: Special effects
    }
}
