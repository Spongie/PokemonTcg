using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Ram : Attack
    {
        public Ram()
        {
            Name = "Ram";
            Description = string.Empty;
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 1),
            };
        }


        public override int GetDamage(Player owner, Player opponent)
        {
            return 10;
        }
    }
}
