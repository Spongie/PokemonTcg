using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Nightmare : Attack
    {
        public Nightmare()
        {
            Name = "Nightmare";
            Description = "The Defending Pokémon is now Asleep.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Psychic, 1),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 10;
        }
		//TODO:
    }
}
