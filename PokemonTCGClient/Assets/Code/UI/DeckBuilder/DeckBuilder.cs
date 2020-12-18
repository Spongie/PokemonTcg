using NetworkingCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TCGCards;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Code.UI.DeckBuilder
{
    public class DeckBuilder : MonoBehaviour
    {
        public static TCGCards.Core.Deck CurrentDeck;
        public InputField deckName;
        public GameObject cardPrefab;
        public GameObject deckContent;

        public TextMeshProUGUI textFieldCount;
        public TextMeshProUGUI textFieldEnergyCount;
        public TextMeshProUGUI textFieldTrainerCount;
        public TextMeshProUGUI textFieldPokemonCount;

        public GameObject ValidationModal;
        public TextMeshProUGUI ValidationText;

        public Dropdown formatDropdown;

        private void Start()
        {
            NetworkId defaultFormatId = null;

            if (CurrentDeck != null)
            {
                var filename = CurrentDeck.Name;
                defaultFormatId = CurrentDeck.FormatId;

                foreach (var character in Path.GetInvalidFileNameChars())
                {
                    filename = filename.Replace(character, '\0');
                }

                var fullPath = Path.Combine(Application.streamingAssetsPath, "Decks", filename + MainMenu.Deck.deckExtension);
                var deck = Serializer.Deserialize<TCGCards.Core.Deck>(File.ReadAllText(fullPath));

                foreach (var card in deck.Cards)
                {
                    AddToDeck(card);
                }

                deckName.text = CurrentDeck.Name;
            }

            if (defaultFormatId == null)
            {
                defaultFormatId = MainMenu.MainMenu.formats.FirstOrDefault(f => f.Name == "Unlimited").Id;
            }

            formatDropdown.options.Clear();
            int index = 0;
            int formatIndex = 1;

            foreach (var format in MainMenu.MainMenu.formats)
            {
                formatDropdown.options.Add(new Dropdown.OptionData
                {
                    text = format.Name
                });

                if (format.Id.Equals(defaultFormatId))
                {
                    formatIndex = index;
                }

                index++;
            }

            formatDropdown.value = formatIndex;
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
                filename = filename.Replace(character, '\0');
            }

            var directory = Path.Combine(Application.streamingAssetsPath, "Decks");

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var format = MainMenu.MainMenu.formats.First(x => x.Name == formatDropdown.options[formatDropdown.value].text);

            var fullPath = Path.Combine(directory, filename);

            var cards = deckContent.GetComponentsInChildren<DeckCard>();

            if (cards.Length == 0)
            {
                return;
            }

            var deck = new TCGCards.Core.Deck
            {
                Cards = new Stack<Card>(cards.Select(deckCard => deckCard.card)),
                FormatId = format.Id,
                Name = deckName.text
            };

            var deckValidation = format.ValidateDeck(deck);

            if (!deckValidation.Result)
            {
                ValidationModal.SetActive(true);
                ValidationText.text = string.Join(Environment.NewLine, deckValidation.Messages);
                return;
            }

            var data = Serializer.Serialize(deck);

            File.WriteAllText(fullPath, data);        
        }

        internal void AddToDeck(Card card)
        {
            var spawnedObject = Instantiate(cardPrefab, deckContent.transform);
            var deckCard = spawnedObject.GetComponent<DeckCard>();

            deckCard.Init(card);
            deckCard.isInDeck = true;

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
