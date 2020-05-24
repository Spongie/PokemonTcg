using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Withdraw : Attack
    {
        public Withdraw()
        {
            Name = "Withdraw";
            Description = "Flip a coin. If heads, prevent all damage done to Squirtle during your opponent's next turn. (Any other effects of attacks still happen.)";
			DamageText = "0";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Water, 1),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (!CoinFlipper.FlipCoin())
            {
                game.GameLog.AddMessage("Flipped tails, nothing happens");
                return;
            }

            game.GameLog.AddMessage("Flipped heads, damage will be prevented");
            game.DamageStoppers.Add(new DamageStopper(() => true));
        }
    }
}
