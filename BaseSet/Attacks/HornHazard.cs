using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class HornHazard : Attack
    {
        public HornHazard()
        {
            Name = "Horn Hazard";
            Description = "Flip a coin. If tails, this attack does nothing.";
			DamageText = "30";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Grass, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            if (!CoinFlipper.FlipCoin())
            {
                return 0;
            }

            return 30;
        }
    }
}
