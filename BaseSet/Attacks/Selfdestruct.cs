using System.Collections.Generic;
using TCGCards;
using TCGCards.Core;

namespace BaseSet.Attacks
{
    internal class Selfdestruct : Attack
    {
        public Selfdestruct()
        {
            Name = "Selfdestruct";
            Description = "Does 10 damage to each Pokémon on each player's Bench. (Don't apply Weakness and Resistance for Benched Pokémon.) Magnemite does 40 damage to itself.";
			DamageText = "40";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Electric, 1),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return 40;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            foreach (var pokemon in opponent.BenchedPokemon)
            {
                pokemon.DamageCounters += 10;
            }

            foreach (var pokemon in owner.BenchedPokemon)
            {
                pokemon.DamageCounters += 10;
            }

            owner.ActivePokemonCard.DamageCounters += 40;
        }
    }
}
