using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Metronome : Attack
    {
        public Metronome()
        {
            Name = "Metronome";
            Description = "Choose 1 of Defending Pokémon's attacks. Metronome copies that attack except for its Energy costs and anything else required in order to use that attack, such as discarding energy cards. (No matter what type the defender is, Clefairy's type is still Colorless.)";
			DamageText = "0";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 3)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }
		//TODO: Special effects
    }
}
