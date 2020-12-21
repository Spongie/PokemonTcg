using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TCGCards.Core
{
    public class Bench : ICollection<PokemonCard>
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

        public PokemonCard[] Pokemons { get; private set; }

        public int Count
        {
            get { return allValidPokemons.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
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

        public IEnumerator<PokemonCard> GetEnumerator()
        {
            return allValidPokemons.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return allValidPokemons.GetEnumerator();
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

        public void Clear()
        {
            allValidPokemons.Clear();
            for (int i = 0; i < Pokemons.Length; i++)
            {
                Pokemons[i] = null;
            }
        }

        public void CopyTo(PokemonCard[] array, int arrayIndex)
        {
            throw new NotImplementedException();
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
                if (Pokemons[i] == pokemon)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
