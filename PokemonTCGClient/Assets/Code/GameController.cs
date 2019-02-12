using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Code._2D;
using NetworkingCore;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code
{
    public enum SpecialGameState
    {
        None,
        SelectingOpponentsPokemon
    }

    public class GameController : MonoBehaviour
    {
        public static GameController Instance;
        public GameField gameField;
        public GameFieldState CurrentGameState;
        public SpecialGameState SpecialState;
        public HandController playerHand;
        public GameObject playerBench;
        public GameObject opponentBench;
        public GameObject playerActivePokemon;
        public GameObject opponentActivePokemon;
        public GameObject cardPrefab;
        public NetworkId myId;
        public GameObject doneButton;
        public GameObject endTurnButton;
        public TextMeshProUGUI playerDeckCountText;
        public TextMeshProUGUI opponentDeckCountText;
        public bool IsMyTurn;

        public GameObject infoPanel;
        public Text infoText;

        private List<Card> selectedCards;
        private Dictionary<GameFieldState, string> gameStateInfo;

        private static GameFieldState[] statesWithDoneAction = new []
        {
            GameFieldState.BothSelectingBench
        };

        private void Awake()
        {
            Instance = this;
            selectedCards = new List<Card>();

            gameStateInfo = new Dictionary<GameFieldState, string>
            {
                { GameFieldState.BothSelectingActive, "Select your active Pokémon" },
                { GameFieldState.BothSelectingBench, "Select your benched Pokémon" },
            };
        }

        private void Start()
        {
            myId = NetworkManager.Instance.Me.Id;
            var messageId = NetworkManager.Instance.gameService.HostGame(NetworkManager.Instance.Me.Id);

            NetworkManager.Instance.RegisterCallback(messageId, OnGameHosted);
            NetworkManager.Instance.RegisterCallback(MessageTypes.GameUpdate, OnGameUpdated);
            NetworkManager.Instance.RegisterCallback(MessageTypes.SelectOpponentPokemon, OnStartSelectingOpponentPokemon);
        }

        private void OnStartSelectingOpponentPokemon(object obj)
        {
            selectedCards.Clear();
            var count = ((SelectOpponentPokemon)obj).Count;
            SpecialState = SpecialGameState.SelectingOpponentsPokemon;
            doneButton.SetActive(true);

            infoPanel.SetActive(true);
            infoText.text = $"Select up to {count} of your oppoents pokemon";
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
                cardController.SetSelected(true);
                selectedCards.Add(cardController.card);
            }
            else if (SpecialState == SpecialGameState.SelectingOpponentsPokemon)
            {
                if (cardController.card.Owner.Id.Equals(myId))
                {
                    return;
                }

                cardController.SetSelected(true);
                selectedCards.Add(cardController.card);
            }
        }

        public void ActivateAbility(Ability ability)
        {
            if (!gameField.ActivePlayer.Id.Equals(myId) || !ability.CanActivate() || ability.PokemonOwner.Owner.Hand.Contains(ability.PokemonOwner))
            {
                return;
            }

            NetworkManager.Instance.gameService.ActivateAbility(ability);
        }

        public void Attack(Attack attack)
        {
            if (!gameField.ActivePlayer.Id.Equals(myId) || !gameField.ActivePlayer.ActivePokemonCard.Attacks.Contains(attack) || !attack.CanBeUsed(gameField, gameField.ActivePlayer, gameField.NonActivePlayer))
            {
                return;
            }

            NetworkManager.Instance.gameService.Attack(attack);
        }

        public void EndTurnButtonClicked()
        {
            NetworkManager.Instance.gameService.EndTurn();
        }

        public void DoneButtonClicked()
        {
            if (gameField.GameState == GameFieldState.BothSelectingBench)
            {
                var id = NetworkManager.Instance.gameService.AddToBench(myId, selectedCards.OfType<PokemonCard>().ToList());
                NetworkManager.Instance.RegisterCallback(id, OnGameUpdated);
                selectedCards.Clear();
            }
            else if (SpecialState == SpecialGameState.SelectingOpponentsPokemon)
            {
                var message = new CardListMessage(selectedCards);
                NetworkManager.Instance.Me.Send(message.ToNetworkMessage(myId));
                SpecialState = SpecialGameState.None;
                infoPanel.SetActive(false);
            }
        }

        private void OnGameUpdated(object message)
        {
            var gameMessage = message is GameFieldMessage ? ((GameFieldMessage)message).Game : (GameField)message;
            Debug.Log("Game updated, handling message");

            GameFieldState oldState = gameField.GameState;
            
            gameField = gameMessage;
            IsMyTurn = gameField.ActivePlayer.Id.Equals(myId);

            doneButton.SetActive(statesWithDoneAction.Contains(gameField.GameState));
            endTurnButton.SetActive(IsMyTurn);

            if (gameField.GameState == GameFieldState.WaitingForConnection)
            {
                return;
            }

            Player me = gameField.Players.First(p => p.Id.Equals(myId));
            Player opponent = gameField.Players.First(p => !p.Id.Equals(myId));

            if (me.Deck != null)
            {
                playerDeckCountText.text = me.Deck.Cards.Count.ToString();
            }
            if (opponent.Deck != null)
            {
                opponentDeckCountText.text = opponent.Deck.Cards.Count.ToString();
            }

            playerHand.SetHand(me.Hand);

            SetActivePokemon(playerActivePokemon, me.ActivePokemonCard);
            SetActivePokemon(opponentActivePokemon, opponent.ActivePokemonCard);

            SetBenchedPokemon(playerBench, me.BenchedPokemon);
            SetBenchedPokemon(opponentBench, opponent.BenchedPokemon);

            if (gameStateInfo.ContainsKey(gameField.GameState))
            {
                infoPanel.SetActive(true);
                infoText.text = gameStateInfo[gameField.GameState];
            }
            else
            {
                infoPanel.SetActive(false);
            }
        }

        private void SetBenchedPokemon(GameObject parent, IEnumerable<PokemonCard> pokemons)
        {
            parent.DestroyAllChildren();

            foreach (var pokemon in pokemons)
            {
                var spawnedCard = Instantiate(cardPrefab, parent.transform);

                var controller = spawnedCard.GetComponentInChildren<CardRenderer>();
                controller.SetCard(pokemon);
                controller.SetIsBenched();
            }
        }

        private void SetActivePokemon(GameObject parent, PokemonCard pokemonCard)
        {
            if (pokemonCard == null)
            {
                return;
            }

            parent.DestroyAllChildren();

            var spawnedCard = Instantiate(cardPrefab, parent.transform);

            var controller = spawnedCard.GetComponentInChildren<CardRenderer>();
            controller.SetCard(pokemonCard);
            controller.SetIsActivePokemon();
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
