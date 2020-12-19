using NetworkingCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TCGCards;
using TCGCards.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Code.UI.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject content;
        public GameObject gamePrefab;
        public Dropdown deckDropDown;
        public static List<Format> formats;
        private List<TCGCards.Core.Deck> decks;

        private void Awake()
        {
            formats = Serializer.Deserialize<List<Format>>(File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "Data", "formats.json")));
        }

        private void Start()
        {
            decks = new List<TCGCards.Core.Deck>();
            RefreshGames();

            deckDropDown.options.Clear();
            var directory = Path.Combine(Application.streamingAssetsPath, "Decks");

            foreach (var file in Directory.GetFiles(directory).Where(f => f.EndsWith(Deck.deckExtension)))
            {
                var deck = Serializer.Deserialize<TCGCards.Core.Deck>(File.ReadAllText(file));

                if (deck.FormatId == null)
                {
                    deck.FormatId = formats.FirstOrDefault(f => f.Name == "Unlimited").Id;
                    deck.Name = new FileInfo(file).Name.Replace(Deck.deckExtension, string.Empty);
                    File.WriteAllText(file, Serializer.Serialize(deck));
                }

                string formatName = formats.FirstOrDefault(f => f.Id.Equals(deck.FormatId)).Name;

                decks.Add(deck);
                deckDropDown.options.Add(new Dropdown.OptionData(new FileInfo(file).Name.Replace(Deck.deckExtension, string.Empty) + " - " + formatName));
            }

            if (deckDropDown.options.Count > 0)
            {
                deckDropDown.value = 1;
            }
        }

        public void RefreshGames()
        {
            var id = NetworkManager.Instance.gameService.GetAvailableGames();
            NetworkManager.Instance.RegisterCallbackById(id, OnGamesRefreshed);
        }

        private void OnGamesRefreshed(object obj)
        {
            content.DestroyAllChildren();

            foreach (var game in ((List<GameInfo>)obj))
            {
                var spawnedObject = Instantiate(gamePrefab, content.transform);
                spawnedObject.GetComponent<Game>().Init(game);
            }
        }

        public void OnHostGameClicked()
        {
            var id = NetworkManager.Instance.gameService.HostGame(NetworkManager.Instance.Me.Id, LoadDeckSelectedDeck());
            NetworkManager.Instance.RegisterCallbackById(id, OnGameHosted);
        }

        private void OnGameHosted(object obj)
        {
            NetworkManager.Instance.CurrentGame = (GameField)obj;
            SceneManager.LoadScene("UI_2D");
        }

        public TCGCards.Core.Deck LoadDeckSelectedDeck()
        {
            var fileName = decks[deckDropDown.value].Name;

            foreach (var character in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(character, '\0');
            }

            var deckFile = Path.Combine(Application.streamingAssetsPath, "Decks", fileName + Deck.deckExtension);

            var deck = Serializer.Deserialize<TCGCards.Core.Deck>(File.ReadAllText(deckFile));

            foreach (var card in deck.Cards)
            {
                card.Id = NetworkId.Generate();
            }

            return deck;
        }

        public void ExitClick()
        {
            Application.Quit();
        }
    }
}
