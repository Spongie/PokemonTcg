using System.Collections.Generic;
using System.Linq;
using Assets.Code._2D;
using NetworkingCore;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;
using UnityEngine;

namespace Assets.Code
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance;
        public GameField gameField;
        public GameFieldState CurrentGameState;
        public HandController playerHand;
        public GameObject playerBench;
        public GameObject opponentBench;
        public GameObject playerActivePokemon;
        public GameObject opponentActivePokemon;
        public GameObject cardPrefab;
        public NetworkId myId;
        public GameObject doneButton;
        private List<Card> selectedBenchCards;
        private static GameFieldState[] statesWithDoneAction = new []
        {
            GameFieldState.BothSelectingBench
        };

        private void Awake()
        {
            Instance = this;
            selectedBenchCards = new List<Card>();
        }

        private void Start()
        {
            myId = NetworkManager.Instance.Me.Id;
            var messageId = NetworkManager.Instance.gameService.HostGame(NetworkManager.Instance.Me.Id);

            NetworkManager.Instance.RegisterCallback(messageId, OnGameHosted);
            NetworkManager.Instance.RegisterCallback(MessageTypes.GameUpdate, OnGameUpdated);
        }

        public void OnCardClicked(CardRenderer cardController)
        {
            if (gameField.GameState == GameFieldState.BothSelectingActive)
            {
                var id = NetworkManager.Instance.gameService.SetActivePokemon(myId, (PokemonCard)cardController.card);
                NetworkManager.Instance.RegisterCallback(id, OnGameUpdated);
            }
            else if (gameField.GameState == GameFieldState.BothSelectingBench)
            {
                cardController.GetComponentInChildren<SelectIndicator>().SetSelected(true);
                selectedBenchCards.Add(cardController.card);
            }
        }

        public void DoneButtonClicked()
        {
            if (gameField.GameState == GameFieldState.BothSelectingBench)
            {
                var id = NetworkManager.Instance.gameService.AddToBench(myId, selectedBenchCards.OfType<PokemonCard>().ToList());
                NetworkManager.Instance.RegisterCallback(id, OnGameUpdated);
                selectedBenchCards.Clear();
            }
        }

        private void OnGameUpdated(object message)
        {
            var gameMessage = message is GameFieldMessage ? ((GameFieldMessage)message).Game : (GameField)message;
            Debug.Log("Game updated, handling message");

            GameFieldState oldState = gameField.GameState;
            
            gameField = gameMessage;

            doneButton.SetActive(statesWithDoneAction.Contains(gameField.GameState));

            if (gameField.GameState == GameFieldState.WaitingForConnection)
            {
                return;
            }

            Player me = gameField.Players.First(p => p.Id.Equals(myId));
            Player opponent = gameField.Players.First(p => !p.Id.Equals(myId));

            playerHand.SetHand(me.Hand);

            SetActivePokemon(playerActivePokemon, me.ActivePokemonCard);
            SetActivePokemon(opponentActivePokemon, opponent.ActivePokemonCard);
        }

        private void SetActivePokemon(GameObject parent, PokemonCard pokemonCard)
        {
            if (pokemonCard == null)
            {
                return;
            }

            var controller = parent.GetComponentInChildren<CardRenderer>();

            if (controller != null)
            {
                controller.SetCard(pokemonCard);
                return;
            }

            controller = Instantiate(cardPrefab, parent.transform).GetComponent<CardRenderer>();

            controller.isActivePokemon = true;
            controller.SetCard(pokemonCard);
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
