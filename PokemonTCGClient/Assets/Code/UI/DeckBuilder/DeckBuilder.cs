using NetworkingCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TCGCards;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Code.UI.DeckBuilder
{
    public class DeckBuilder : MonoBehaviour
    {
        public static string CurrentDeck;
        public InputField deckName;
        public GameObject cardPrefab;
        public GameObject deckContent;

        private void Start()
        {
            if (!string.IsNullOrWhiteSpace(CurrentDeck))
            {
                var fullPath = Path.Combine(Application.streamingAssetsPath, "Decks", CurrentDeck + ".dck");
                var deck = Serializer.Deserialize<List<TypeInfo>>(File.ReadAllText(fullPath));

                foreach (var type in deck)
                {
                    var card = Card.CreateFromTypeInfo(type);

                    if (card != null)
                    {
                        AddToDeck(card);
                    }
                }

                deckName.text = CurrentDeck;
            }
        }

        public void OnExitClick()
        {
            CurrentDeck = null;
            SceneManager.LoadScene("MainMenu");
        }

        public void OnSaveClick()
        {
            var filename = deckName.text + ".dck";

            foreach (var character in Path.GetInvalidFileNameChars())
            {
                filename.Replace(character, '\0');
            }

            var directory = Path.Combine(Application.streamingAssetsPath, "Decks");

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var fullPath = Path.Combine(directory, filename);

            var cards = deckContent.GetComponentsInChildren<DeckCard>().Select(deckCard => deckCard.card.GetType().GetTypeInfo()).ToList();

            var data = Serializer.Serialize(cards);

            File.WriteAllText(fullPath, data);        
        }

        internal void AddToDeck(Card card)
        {
            var spawnedObject = Instantiate(cardPrefab, deckContent.transform);
            var deckCard = spawnedObject.GetComponent<DeckCard>();

            deckCard.Init(card);
            deckCard.isInDeck = true;

            spawnedObject.GetComponentInChildren<CardZoomer>().zoomMode = ZoomMode.FromTopLeft;
        }
    }
}
