using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class SurpriseThunder : Attack
    {
        public SurpriseThunder()
        {
            Name = "Surprise Thunder";
            Description = "Flip a coin. If heads, flip another coin. If the second coin is heads, this attack does 20 damage to each of your opponent&#8217;s Benched Pokémon. If the second coin is tails, this attack does 10 damage to each of your opponent&#8217;s Benched Pokémon. (Don&#8217;t apply Weakness and Resistance for Benched Pokémon.)";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Electric, 3)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 30;
        }
		//TODO:
    }
}
