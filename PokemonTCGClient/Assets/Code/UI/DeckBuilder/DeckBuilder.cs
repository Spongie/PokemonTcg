using NetworkingCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public Text textFieldCount;
        public Text textFieldEnergyCount;
        public Text textFieldTrainerCount;
        public Text textFieldPokemonCount;

        private void Start()
        {
            if (!string.IsNullOrWhiteSpace(CurrentDeck))
            {
                var fullPath = Path.Combine(Application.streamingAssetsPath, "Decks", CurrentDeck + MainMenu.Deck.deckExtension);
                var deck = Serializer.Deserialize<TCGCards.Core.Deck>(File.ReadAllText(fullPath));

                foreach (var card in deck.Cards)
                {
                    AddToDeck(card);
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
            var filename = deckName.text + MainMenu.Deck.deckExtension;

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

            var deck = new TCGCards.Core.Deck
            {
                Cards = new Stack<Card>(deckContent.GetComponentsInChildren<DeckCard>().Select(deckCard => deckCard.card))
            };

            var data = Serializer.Serialize(deck);

            File.WriteAllText(fullPath, data);        
        }

        internal void AddToDeck(Card card)
        {
            var spawnedObject = Instantiate(cardPrefab, deckContent.transform);
            var deckCard = spawnedObject.GetComponent<DeckCard>();

            deckCard.Init(card);
            deckCard.isInDeck = true;

            spawnedObject.GetComponentInChildren<CardZoomer>().zoomMode = ZoomMode.FromTopLeft;

            UpdateCountInfo();
        }

        public void UpdateCountInfo()
        {
            var cards = deckContent.GetComponentsInChildren<DeckCard>().Select(x => x.card);
            textFieldCount.text = cards.Count().ToString();
            textFieldEnergyCount.text = cards.OfType<EnergyCard>().Count().ToString();
            textFieldTrainerCount.text = cards.OfType<TrainerCard>().Count().ToString();
            textFieldPokemonCount.text = cards.OfType<PokemonCard>().Count().ToString();
        }
    }
}
