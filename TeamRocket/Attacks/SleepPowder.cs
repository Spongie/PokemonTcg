using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class SleepPowder : Attack
    {
        public SleepPowder()
        {
            Name = "Sleep Powder";
            Description = "The Defending Pokémon is now Asleep.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Grass, 1)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 0;
        }
		//TODO:
    }
}
