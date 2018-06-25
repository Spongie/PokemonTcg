using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class FickleAttack : Attack
    {
        public FickleAttack()
        {
            Name = "Fickle Attack";
            Description = "Flip a coin. If tails, this attack does nothing.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Psychic, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return CoinFlipper.FlipCoin() ? 40 : 0;
        }
    }
}
