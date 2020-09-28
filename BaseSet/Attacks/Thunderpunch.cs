using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Thunderpunch : Attack
    {
        public Thunderpunch()
        {
            Name = "Thunderpunch";
            Description = "Flip a coin. If heads, this attack does 30 damage plus 10 more damage; if tails, this attack does 30 damage and Electabuzz does 10 damage to itself.";
			DamageText = "30";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Electric, 1),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            if (CoinFlipper.FlipCoin())
            {
                return 40;
            }
            else
            {
                owner.ActivePokemonCard.DamageCounters += 10;
                return 30;
            }
        }
    }
}
