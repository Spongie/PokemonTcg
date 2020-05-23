using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class SuperFang : Attack
    {
        public SuperFang()
        {
            Name = "Super Fang";
            Description = "Does damage to the Defending Pokémon equal to half the Defending Pokémon's remaining HP (rounded up to the nearest 10).";
			DamageText = "?";
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
