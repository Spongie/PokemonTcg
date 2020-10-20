using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.SpecialAbilities;

namespace BaseSet.Attacks
{
    internal class Scrunch : Attack
    {
        public Scrunch()
        {
            Name = "Scrunch";
            Description = "Flip a coin. If heads, prevent all damage done to Chansey during your opponent's next turn. (Any other effects of attacks still happen.)";
			DamageText = "0";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Colorless, 2)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
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
