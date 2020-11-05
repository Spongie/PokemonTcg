using NetworkingCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCGCards;
using UnityEngine;

namespace Assets.Code.UI.DeckBuilder
{
    public class CardLoader
    {
        public static IEnumerable<PokemonCard> LoadPokemonCards()
        {
            var json = Path.Combine(Application.streamingAssetsPath, "Data", "pokemon.json");

            return Serializer.Deserialize<List<PokemonCard>>(File.ReadAllText(json)).Where(x => x.Completed);
        }

        public static IEnumerable<EnergyCard> LoadEnergyCards()
        {
            var json = Path.Combine(Application.streamingAssetsPath, "Data", "energy.json");

            return Serializer.Deserialize<List<EnergyCard>>(File.ReadAllText(json)).Where(x => x.Completed);
        }

        public static IEnumerable<TrainerCard> LoadTrainerCards()
        {
            var json = Path.Combine(Application.streamingAssetsPath, "Data", "trainers.json");

            return Serializer.Deserialize<List<TrainerCard>>(File.ReadAllText(json)).Where(x => x.Completed);
        }

        public static IEnumerable<Card> LoadAllCards()
        {
            foreach (var pokemon in LoadPokemonCards())
            {
                yield return pokemon;
            }

            foreach (var trainer in LoadTrainerCards())
            {
                yield return trainer;
            }

            foreach (var energy in LoadEnergyCards())
            {
                yield return energy;
            }
        }
    }
}
