using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Fireball : Attack
    {
        public Fireball()
        {
            Name = "Fireball";
            Description = "Use this attack only if there are any [R] Energy cards attached to Dark Charmeleon. Flip a coin. If heads, discard 1 of those Energy cards. If tails, this attack does nothing (not even damage).";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fire, 3)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 70;
        }
		//TODO:
    }
}
