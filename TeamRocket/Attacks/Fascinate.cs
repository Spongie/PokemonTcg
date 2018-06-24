using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Fascinate : Attack
    {
        public Fascinate()
        {
            Name = "Fascinate";
            Description = "Flip a coin. If heads, choose 1 of your opponent&#8217;s Benched Pokémon and switch it with the Defending Pokémon. This attack can&#8217;t be used if your opponent has no Benched Pokémon.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }
		//TODO:
    }
}
