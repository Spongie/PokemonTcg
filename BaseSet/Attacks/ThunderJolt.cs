using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class ThunderJolt : Attack
    {
        public ThunderJolt()
        {
            Name = "Thunder Jolt";
            Description = "Flip a coin. If tails, Pikachu does 10 damage to itself.";
			DamageText = "30";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Electric, 1),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 30;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (!CoinFlipper.FlipCoin())
            {
                owner.ActivePokemonCard.DamageCounters += 10;
            }
        }
    }
}
