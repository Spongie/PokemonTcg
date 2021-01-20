using System.Collections.Generic;
using Newtonsoft.Json;

namespace TCGCards.Core
{
    public class Bench
    {
        [JsonProperty]
        private List<PokemonCard> allValidPokemons;

        public Bench()
        {
            Pokemons = new PokemonCard[GameField.BenchMaxSize];
            allValidPokemons = new List<PokemonCard>(5);
        }

        public Bench(IEnumerable<PokemonCard> pokemonCards) :this()
        {
            int index = 0;

            foreach (var pokemon in pokemonCards)
            {
                Pokemons[index] = pokemon;
                allValidPokemons.Add(pokemon);
                index++;
            }
        }

        public PokemonCard[] Pokemons { get; set; }

        public IEnumerable<PokemonCard> ValidPokemonCards
        {
            get
            {
                return allValidPokemons;
            }
        }

        public int Count
        {
            get { return allValidPokemons.Count; }
        }

        public bool Contains(PokemonCard pokemon)
        {
            for (int i = 0; i < Pokemons.Length; i++)
            {
                if (Pokemons[i] == pokemon)
                {
                    return true;
                }
            }

            return false;
        }

        public void ReplaceWith(PokemonCard oldPokemon, PokemonCard newPokemon)
        {
            for (int i = 0; i < Pokemons.Length; i++)
            {
                if (Pokemons[i] == oldPokemon)
                {
                    Pokemons[i] = newPokemon;
                    allValidPokemons.Remove(oldPokemon);
                    allValidPokemons.Add(newPokemon);
                    break;
                }
            }
        }

        public void Add(PokemonCard pokemon)
        {
            for (int i = 0; i < Pokemons.Length; i++)
            {
                if (Pokemons[i] != null)
                {
                    continue;
                }

                Pokemons[i] = pokemon;
                allValidPokemons.Add(pokemon);
                break;
            }
        }

        public int GetNextFreeIndex()
        {
            for (int i = 0; i < Pokemons.Length; i++)
            {
                if (Pokemons[i] == null)
                {
                    return i;
                }
            }

            return -1;
        }

        public PokemonCard GetFirst()
        {
            for (int i = 0; i < Pokemons.Length; i++)
            {
                if (Pokemons[i] != null)
                {
                    return Pokemons[i];
                }
            }

            return null;
        }


        public bool Remove(PokemonCard pokemon)
        {
            for (int i = 0; i < Pokemons.Length; i++)
            {
                if (Pokemons[i] == pokemon)
                {
                    Pokemons[i] = null;
                    allValidPokemons.Remove(pokemon);
                    return true;
                }
            }

            return false;
        }

        public int IndexOf(PokemonCard pokemon)
        {
            for (int i = 0; i < Pokemons.Length; i++)
            {
                if (Pokemons[i] != null && Pokemons[i].Id.Equals(pokemon.Id))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
