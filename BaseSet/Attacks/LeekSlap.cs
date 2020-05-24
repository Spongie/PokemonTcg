using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class LeekSlap : Attack
    {
        private bool used;

        public LeekSlap()
        {
            Name = "Leek Slap";
            Description = "Flip a coin. If tails, this attack does nothing. Either way, you can't use this attack again as long as Farfetch'd stays in play (even putting Farfetch'd on the Bench won't let you use it again.)";
			DamageText = "30";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            used = true;

            if (!CoinFlipper.FlipCoin())
            {
                return 0;
            }

            return 30;
        }

        public override bool CanBeUsed(GameField game, Player owner, Player opponent)
        {
            return !used && base.CanBeUsed(game, owner, opponent);
        }
    }
}
