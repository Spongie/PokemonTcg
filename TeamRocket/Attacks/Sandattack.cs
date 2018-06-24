using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Sandattack : Attack
    {
        public Sandattack()
        {
            Name = "Sand-attack";
            Description = "If the Defending Pokémon tries to attack during your opponent&#8217;s next turn, your opponent flips a coin. If tails, that attack does nothing.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 10;
        }
		//TODO:
    }
}
