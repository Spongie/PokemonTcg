using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class ElectricShock : Attack
    {
        public ElectricShock()
        {
            Name = "Electric Shock";
            Description = "Flip a coin. If tails, Electrode does 10 damage to itself.";
			DamageText = "50";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Electric, 3)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 50;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (!CoinFlipper.FlipCoin())
            {
                game.GameLog.AddMessage($"Coin flipped tails {owner.ActivePokemonCard.GetName()} deals 10 damage to itself");
                owner.ActivePokemonCard.DamageCounters += 10;
            }
            else
            {
                game.GameLog.AddMessage("Coin flipped heads, nothing happened");
            }
        }
    }
}
