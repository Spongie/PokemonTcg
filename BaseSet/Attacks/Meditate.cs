using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Meditate : Attack
    {
        public Meditate()
        {
            Name = "Meditate";
            Description = "Does 20 damage plus 10 more damage for each damage counter on the Defending Pokémon.";
			DamageText = "20";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Psychic, 2),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 20;
        }
		//TODO: Special effects
    }
}
