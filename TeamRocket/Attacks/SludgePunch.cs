using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class SludgePunch : Attack
    {
        public SludgePunch()
        {
            Name = "Sludge Punch";
            Description = "The Defending Pokémon is now Poisoned.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Grass, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 20;
        }
		//TODO:
    }
}
