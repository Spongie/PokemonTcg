using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class PoisonVapor : Attack
    {
        public PoisonVapor()
        {
            Name = "Poison Vapor";
            Description = "The Defending Pokémon is now Poisoned. This attack does 10 damage to each of your opponent&#8217;s Benched Pokémon. (Don&#8217;t apply Weakness and Resistance for Benched Pokémon.)";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Grass, 3)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 10;
        }
		//TODO:
    }
}
