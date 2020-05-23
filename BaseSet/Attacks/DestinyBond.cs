using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class DestinyBond : Attack
    {
        public DestinyBond()
        {
            Name = "Destiny Bond";
            Description = "Discard 1 Energy card attached to Gastly in order to use this attack. If a Pokémon Knocks Out Gastly during your opponent's next turn, Knock Out that Pokémon.";
			DamageText = "0";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Psychic, 1),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }
		//TODO: Special effects
    }
}
