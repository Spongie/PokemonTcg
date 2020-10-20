using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class TailSlap : Attack
    {
        public TailSlap()
        {
            Name = "Tail Slap";
            Description = "";
            DamageText = "20";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 20;
        }
		
    }
}
