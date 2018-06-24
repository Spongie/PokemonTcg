using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Poisonpowder : Attack
    {
        public Poisonpowder()
        {
            Name = "Poisonpowder";
            Description = "The Defending Pokémon is now Poisoned.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Grass, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }
		//TODO:
    }
}
