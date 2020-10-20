using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Bite30 : Attack
    {
        public Bite30()
        {
            Name = "Bite";
            Description = string.Empty;
            DamageText = "30";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 1),
                new Energy(EnergyTypes.Grass, 1)
            };
        }


        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 30;
        }
    }
}
