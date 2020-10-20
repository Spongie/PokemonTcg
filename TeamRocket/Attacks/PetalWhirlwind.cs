using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class PetalWhirlwind : Attack
    {
        public PetalWhirlwind()
        {
            Name = "Petal Whirlwind";
            Description = "30× damage. Flip 3 coins. This attack does 30 damage times the number of heads. If you get 2 or more heads, Dark Vileplume is now Confused (after doing damage).";
            DamageText = "30x";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Grass, 3)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            var heads = CoinFlipper.FlipCoins(3);

            if (heads >= 2)
                owner.ActivePokemonCard.IsConfused = true;

            return heads * 30;
        }
    }
}
