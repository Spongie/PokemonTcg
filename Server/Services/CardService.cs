﻿using Entities.Models;
using NetworkingCore;
using System.Collections.Generic;
using System.IO;
using TCGCards;

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
            cards.AddRange(pokemonCards);
            cards.AddRange(Serializer.Deserialize<List<EnergyCard>>(File.ReadAllText("energy.json")));

            Logger.Instance.Log($"Loaded {cards.Count} cards to cache");
        }

        public List<Card> GetAllCards() => cards;
        public List<Set> GetAllSets() => sets;
    }
}
