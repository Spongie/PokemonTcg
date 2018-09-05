using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class MassExplosion : Attack
    {
        public MassExplosion()
        {
            Name = "Mass Explosion";
            Description = "20× damage. Does 20 damage times the number of Koffings, Weezings, and Dark Weezings in play. (Apply Weakness and Resistance.) Then, this attack does 20 damage to each Koffing, Weezing, and Dark Weezing (even your own). Don't apply Weakness and Resistance.";
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Grass, 1),
				new Energy(EnergyTypes.Colorless, 1)
            };
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            var pokemonNames = new[] { PokemonNames.Koffing, PokemonNames.Weezing, PokemonNames.DarkWeezing };
            var validPokemons = owner.BenchedPokemon.Where(pokemon => pokemonNames.Contains(pokemon.PokemonName)).ToList();
            validPokemons.Add(owner.ActivePokemonCard);
            
            if (pokemonNames.Contains(opponent.ActivePokemonCard.PokemonName))
                validPokemons.Add(opponent.ActivePokemonCard);

            validPokemons.AddRange(opponent.BenchedPokemon.Where(pokemon => pokemonNames.Contains(pokemon.PokemonName)));

            foreach (var pokemon in validPokemons)
            {
                pokemon.DamageCounters += 20;
            }

            return 20 * validPokemons.Count;
        }
    }
}
