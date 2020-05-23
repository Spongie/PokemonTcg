using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Lure : Attack
    {
        public Lure()
        {
            Name = "Lure";
            Description = "If your opponent has any Benched Pokémon, choose 1 of them and switch it with the Defending Pokémon.";
			DamageText = "0";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }
		//TODO: Special effects
    }
}
