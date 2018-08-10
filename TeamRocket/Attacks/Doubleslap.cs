using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class Doubleslap : Attack
    {
        public Doubleslap()
        {
            Name = "Doubleslap";
            Description = "10× damage. Flip 2 coins. This attack does 10 damage times the number of heads.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Water, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return CoinFlipper.FlipCoins(2) * 10;
        }
    }
}
