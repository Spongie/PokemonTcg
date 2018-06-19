using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class ContinuousFireball : Attack
    {
        public ContinuousFireball()
        {
            Name = "Continuous Fireball";
            Description = "50× damage. Flip a number of coins equal to the number for [R] Energy cards attached to Dark Charizard. This attack does 50 damage times the number of heads. Discard a number of [R] Energy cards attached to Dark Charizard equal to the number of heads.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Fire, 2)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 0;
        }
		//TODO:
    }
}
