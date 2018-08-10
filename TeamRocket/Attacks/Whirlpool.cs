using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Whirlpool : Attack
    {
        public Whirlpool()
        {
            Name = "Whirlpool";
            Description = "If the Defending Pokémon has any Energy cards attached to it, choose 1 of them and discard it.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Water, 2),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 20;
        }
		//TODO:
    }
}
