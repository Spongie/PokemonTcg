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
        private Dictionary<NetworkId, Card> cards;
        private List<Set> sets;

        public void InitTypes()
        {
            Logger.Instance.Log("Loading sets cache");

            sets = Serializer.Deserialize<List<Set>>(File.ReadAllText("sets.json"));

            Logger.Instance.Log($"Loaded {sets.Count} sets to cache");
            Logger.Instance.Log("Loading cards cache");

            string json = File.ReadAllText("pokemon.json");

            var pokemonCards = Serializer.Deserialize<List<PokemonCard>>(json);

            var cardList = new List<Card>();
            cardList.AddRange(pokemonCards.Where(card => card.Completed));
            cardList.AddRange(Serializer.Deserialize<List<EnergyCard>>(File.ReadAllText("energy.json")).Where(card => card.Completed));
            cardList.AddRange(Serializer.Deserialize<List<TrainerCard>>(File.ReadAllText("trainers.json")).Where(card => card.Completed));

            cards = cardList.ToDictionary(card => card.CardId);

            Logger.Instance.Log($"Loaded {cards.Count} cards to cache");
        }

        public List<Card> GetAllCards() => cards.Values.ToList();
        public List<Set> GetAllSets() => sets;

        public bool UpdateCards(string pokemonCards, string energyCards, string tainerCards, string sets, string formats)
        {
            Logger.Instance.Log("Received card updates, updating cards...");

            File.WriteAllText("pokemon.json", pokemonCards);
            File.WriteAllText("energy.json", energyCards);
            File.WriteAllText("trainers.json", tainerCards);
            File.WriteAllText("sets.json", sets);
            File.WriteAllText("formats.json", formats);

            Logger.Instance.Log("reloading caches...");
            InitTypes();

            var version = new VersionNumber(File.ReadAllText("cards.version"));
            version.Build++;
            File.WriteAllText("cards.version", version.ToString());

            Logger.Instance.Log("Update complete");

            return true;
        }

        public Card CreateCardById(NetworkId id)
        {
            if (!cards.ContainsKey(id))
            {
                Logger.Instance.Log($"Card with id {id} not found");
                return null;
            }

            return cards[id].Clone();
        }
    }
}
