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
            DamageText = "20";
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 20;
        }
    }
}
