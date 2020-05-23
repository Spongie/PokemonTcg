using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Psychic : Attack
    {
        public Psychic()
        {
            Name = "Psychic";
            Description = "Does 10 damage plus 10 more damage for each Energy card attached to the Defending Pokémon.";
			DamageText = "10";
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
		//TODO: Special effects
    }
}
