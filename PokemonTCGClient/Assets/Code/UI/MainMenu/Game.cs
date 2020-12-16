﻿using System;
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
            var id = NetworkManager.Instance.gameService.JoinTheActiveGame(NetworkManager.Instance.Me.Id, game.Id, menu.LoadDeckSelectedDeck());
            NetworkManager.Instance.RegisterCallbackById(id, OnGameJoined);
        }

        private void OnGameJoined(object obj)
        {
            NetworkManager.Instance.CurrentGame = (GameField)obj;
            SceneManager.LoadScene("UI_2D");
        }
    }
}
