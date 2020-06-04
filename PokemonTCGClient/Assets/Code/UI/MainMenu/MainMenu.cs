using NetworkingCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

        private void Awake()
        {
            new TeamRocket.Set();
            new BaseSet.Set();
        }

        private void Start()
        {
            RefreshGames();

            deckDropDown.options.Clear();
            var directory = Path.Combine(Application.streamingAssetsPath, "Decks");

            foreach (var file in Directory.GetFiles(directory).Where(f => !f.EndsWith(".meta")))
            {
                deckDropDown.options.Add(new Dropdown.OptionData(new FileInfo(file).Name.Replace(".dck", string.Empty)));
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

        public List<TypeInfo> LoadDeckSelectedDeck()
        {
            var deck = Path.Combine(Application.streamingAssetsPath, "Decks", deckDropDown.options[deckDropDown.value].text + ".dck");

            return Serializer.Deserialize<List<TypeInfo>>(File.ReadAllText(deck));
        }
    }
}
