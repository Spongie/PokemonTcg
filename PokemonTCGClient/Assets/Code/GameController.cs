using System.Linq;
using NetworkingCore;
using TCGCards.Core;
using TCGCards.Core.Messages;
using UnityEngine;

namespace Assets.Code
{
    public class GameController : MonoBehaviour
    {
        public GameField gameField;
        public GameFieldState CurrentGameState;
        public HandController playerHand;
        public HandController opponentHand;
        public GameObject playerBench;
        public GameObject opponentBench;
        public GameObject playerActivePokemon;
        public GameObject opponentActivePokemon;
        public NetworkId myId;

        private void Start()
        {
            myId = NetworkManager.Instance.Me.Id;
            var messageId = NetworkManager.Instance.gameService.HostGame(NetworkManager.Instance.Me.Id);

            NetworkManager.Instance.RegisterCallback(messageId, OnGameHosted);
            NetworkManager.Instance.RegisterCallback(MessageTypes.GameUpdate, OnGameUpdated);
        }

        private void OnGameUpdated(object game)
        {
            gameField = ((GameFieldMessage)game).Game;

            if (gameField.GameState == GameFieldState.WaitingForConnection)
            {
                return;
            }

            playerHand.SetHand(gameField.Players.First(p => p.Id.Equals(myId)).Hand);
            opponentHand.SetHand(gameField.Players.First(p => !p.Id.Equals(myId)).Hand);
        }

        private void OnGameHosted(object param1)
        {
            gameField = (GameField)param1;
        }

        private void Update()
        {
            if (gameField == null)
            {
                return;
            }

            CurrentGameState = gameField.GameState;

            switch (gameField.GameState)
            {
                case GameFieldState.WaitingForConnection:
                    break;
                case GameFieldState.WaitingForRegistration:
                    break;
                case GameFieldState.BothSelectingActive:
                    break;
                case GameFieldState.BothSelectingBench:
                    break;
                case GameFieldState.TurnStarting:
                    break;
                case GameFieldState.ActivePlayerSelectingFromBench:
                    break;
                case GameFieldState.UnActivePlayerSelectingFromBench:
                    break;
                case GameFieldState.ActivePlayerSelectingPrize:
                    break;
                case GameFieldState.UnActivePlayerSelectingPrize:
                    break;
                case GameFieldState.InTurn:
                    break;
                case GameFieldState.WaitingForDeckSearch:
                    break;
                case GameFieldState.EndAttack:
                    break;
                default:
                    break;
            }
        }
    }
}
