using Entities.Models;
using NetworkingCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TCGCards;
using TCGCards.Core;

namespace Server.Services
{
    public class CardService : IService
    {
        private List<Card> cards;
        private List<Set> sets;

        public void InitTypes()
        {
            Logger.Instance.Log("Loading sets cache");

            sets = Serializer.Deserialize<List<Set>>(File.ReadAllText("sets.json"));

            Logger.Instance.Log($"Loaded {sets.Count} sets to cache");
            Logger.Instance.Log("Loading cards cache");

            string json = File.ReadAllText("pokemon.json");

            var pokemonCards = Serializer.Deserialize<List<PokemonCard>>(json);

            cards = new List<Card>();
            cards.AddRange(pokemonCards.Where(card => card.Completed));
            cards.AddRange(Serializer.Deserialize<List<EnergyCard>>(File.ReadAllText("energy.json")).Where(card => card.Completed));
            cards.AddRange(Serializer.Deserialize<List<TrainerCard>>(File.ReadAllText("trainers.json")).Where(card => card.Completed));

            Logger.Instance.Log($"Loaded {cards.Count} cards to cache");
        }

        public List<Card> GetAllCards() => cards;
        public List<Set> GetAllSets() => sets;

        public bool UpdateCards(string pokemonCards, string energyCards, string tainerCards, string sets)
        {
            Logger.Instance.Log("Received card updates, updating cards...");

            File.WriteAllText("pokemon.json", pokemonCards);
            File.WriteAllText("energy.json", energyCards);
            File.WriteAllText("trainers.json", tainerCards);
            File.WriteAllText("sets.json", sets);

            Logger.Instance.Log("reloading caches...");
            InitTypes();

            var version = new VersionNumber(File.ReadAllText("cards.version"));
            version.Minor++;
            File.WriteAllText("cards.version", version.ToString());

            Logger.Instance.Log("Update complete");

            return true;
        }
    }
}
