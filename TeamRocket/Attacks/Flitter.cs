using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Flitter : Attack
    {
        public Flitter()
        {
            Name = "Flitter";
            Description = "Choose 1 of your opponent&#8217;s Pokémon. This attack does 20 damage to that Pokémon. Don&#8217;t apply Weakness and Resistance for this attack. (Any other effects that would happen after applying Weakness and Resistance still happen.)";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Grass, 2)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 0;
        }
		//TODO:
    }
}
