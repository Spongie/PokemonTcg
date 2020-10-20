using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Doubleslap : Attack
    {
        public Doubleslap()
        {
            Name = "Doubleslap";
            Description = "Flip 2 coins. This attack does 30 damage times number of heads.";
			DamageText = "30";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Water, 2),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            var heads = game.FlipCoins(2);

            return  heads * 30;
        }
    }
}
