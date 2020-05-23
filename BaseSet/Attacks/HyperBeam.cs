using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class HyperBeam : Attack
    {
        public HyperBeam()
        {
            Name = "Hyper Beam";
            Description = "If the Defending Pokémon has any Energy cards attached to it, choose 1 of them and discard it.";
			DamageText = "20";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 4)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 20;
        }
		//TODO: Special effects
    }
}
