using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class LightningFlash : Attack
    {
        public LightningFlash()
        {
            Name = "Lightning Flash";
            Description = "If the Defending Pokémon tries to attack during your opponent&#8217;s next turn, your opponent flips a coin. If tails, that attack does nothing.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Electric, 1)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 20;
        }
		//TODO:
    }
}
