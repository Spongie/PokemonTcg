using System;
using System.Linq;
using TCGCards.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Code.UI.MainMenu
{
    public class Game : MonoBehaviour
    {
        private GameInfo game;
        public Text hostedName;
        public Text gameFormat;
        public Text formatName;

        public void Init(GameInfo game)
        {
            this.game = game;
            hostedName.text = game.HostingPlayer;
            gameFormat.text = game.FormatName;
            formatName.text = MainMenu.formats.FirstOrDefault(f => f.Id.Equals(game.Format)).Name;
        }

        public void OnJoinGameClick()
        {
            var menu = GameObject.FindGameObjectWithTag("mainMenu").GetComponent<MainMenu>();
            var selectedDeck = menu.LoadDeckSelectedDeck();

            if (!selectedDeck.FormatId.Equals(game.Format))
            {
                var deckFormatName = MainMenu.formats.First(x => x.Id.Equals(selectedDeck.FormatId)).Name;
                ModalHandler.Instance.DisplayMessage($"You tried to join a {game.FormatName} game with a {deckFormatName} deck");
                return;
            }

            var id = NetworkManager.Instance.gameService.JoinTheActiveGame(NetworkManager.Instance.Me.Id, game.Id, selectedDeck);
            NetworkManager.Instance.RegisterCallbackById(id, OnGameJoined);
        }

        private void OnGameJoined(object obj)
        {
            NetworkManager.Instance.CurrentGame = (GameField)obj;
            SceneManager.LoadScene("UI_2D");
        }
    }
}
