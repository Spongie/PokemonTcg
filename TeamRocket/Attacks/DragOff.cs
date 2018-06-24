using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class DragOff : Attack
    {
        public DragOff()
        {
            Name = "Drag Off";
            Description = "Before doing damage, choose 1 of your opponent&#8217;s Benched Pokémon and switch it with the Defending Pokémon. Do the damage to the new Defending Pokémon. This attack can&#8217;t be used if your opponent has not Benched Pokémon.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fighting, 2),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 20;
        }
		//TODO:
    }
}
