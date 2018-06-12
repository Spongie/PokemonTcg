using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class SpeedBall : Attack
    {
        public SpeedBall()
        {
            Name = "Speed ball";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Electric, 1)
            };
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 20;
        }
    }
}
