using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class TeleportBlast : Attack
    {
        public TeleportBlast()
        {
            Name = "Teleport Blast";
            Description = "You may switch Dark Alakazam with 1 of your Benched Pokémon. (Do the damage before switching the Pokémon.)";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Psychic, 2),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 30;
        }
		//TODO:
    }
}
