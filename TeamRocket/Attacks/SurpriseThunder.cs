using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class SurpriseThunder : Attack
    {
        public SurpriseThunder()
        {
            Name = "Surprise Thunder";
            Description = "Flip a coin. If heads, flip another coin. If the second coin is heads, this attack does 20 damage to each of your opponent's Benched Pokémon. If the second coin is tails, this attack does 10 damage to each of your opponent's Benched Pokémon. (Don't apply Weakness and Resistance for Benched Pokémon.)";
            DamageText = "30";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Electric, 3)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 30;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (!CoinFlipper.FlipCoin())
            {
                return;
            }

            int damage = CoinFlipper.FlipCoin() ? 20 : 10;

            foreach (var pokemon in opponent.BenchedPokemon)
            {
                pokemon.DamageCounters += damage;
            }
        }
    }
}
