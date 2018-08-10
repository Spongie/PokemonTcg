using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class MagneticLines : Attack
    {
        public MagneticLines()
        {
            Name = "Magnetic Lines";
            Description = "If the Defending Pokémon has any basic Energy cards attached to it, choose 1 of them. If your opponent have any Benched Pokémon, choose 1 of them and attach that Energy card to it.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Electric, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 30;
        }
		//TODO:
    }
}
