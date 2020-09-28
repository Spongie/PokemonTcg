using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class RollingTackle : Attack
    {
        public RollingTackle()
        {
            Name = "Rolling Tackle";
            Description = "";
            DamageText = "10";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 10;
        }
		
    }
}
