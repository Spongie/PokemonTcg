using System.Collections.Generic;
using System.IO;
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

        private void Start()
        {
            RefreshGames();

            var directory = Path.Combine(Application.streamingAssetsPath, "Decks");

            foreach (var file in Directory.GetFiles(directory))
            {
                deckDropDown.options.Add(new Dropdown.OptionData(new FileInfo(file).Name));
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
            var id = NetworkManager.Instance.gameService.HostGame(NetworkManager.Instance.Me.Id);
            NetworkManager.Instance.RegisterCallbackById(id, OnGameHosted);
        }

        private void OnGameHosted(object obj)
        {
            NetworkManager.Instance.CurrentGame = (GameField)obj;
            SceneManager.LoadScene("UI_2D");
        }
    }
}
