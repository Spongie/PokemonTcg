using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Code._2D;
using Assets.Code.UI.Game;
using NetworkingCore;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.Code
{
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
        public GameObject selectColorPanel;
        public GameObject selectFromListPanel;
        public GameObject cardPrefab;
        public NetworkId myId;
        public GameObject doneButton;
        public GameObject endTurnButton;
        public Sprite CardBack;

        public bool IsMyTurn;

        public Text infoText;

        private int minSelectedCardCount;
        private List<Card> selectedCards;
        private Dictionary<GameFieldState, string> gameStateInfo;
        private Dictionary<GameFieldState, Action<CardRenderer>> onClickHandlers;
        private Dictionary<SpecialGameState, Action<CardRenderer>> onSpecialClickHandlers;
        private Queue<EnergyCard> energyCardsToAttach;
        private Dictionary<NetworkId, NetworkId> energyPokemonMap;
        private EnergyCard currentEnergyCard;

        private static GameFieldState[] statesWithDoneAction = new[]
        {
            GameFieldState.BothSelectingBench
        };

        private void Awake()
        {
            Instance = this;
            selectedCards = new List<Card>();
            energyCardsToAttach = new Queue<EnergyCard>();
            energyPokemonMap = new Dictionary<NetworkId, NetworkId>();

            InitGamestateInfo();
            RegisterClickHandlers();
        }

        internal void StartAttachingEnergy(EnergyCard card)
        {
            SpecialState = SpecialGameState.AttachingEnergyToPokemon;
            currentEnergyCard = card;
            infoText.text = "Select a pokemon to attach your energy to";
        }

        private void InitGamestateInfo()
        {
            gameStateInfo = new Dictionary<GameFieldState, string>
            {
                { GameFieldState.BothSelectingActive, "Select your active Pokémon" },
                { GameFieldState.BothSelectingBench, "Select your benched Pokémon" },
            };
        }

        private void RegisterClickHandlers()
        {
            onClickHandlers = new Dictionary<GameFieldState, Action<CardRenderer>>
            {
                { GameFieldState.BothSelectingActive, SetActiveStateClicked },
                { GameFieldState.BothSelectingBench, BothSelectingBenchClicked }
            };

            onSpecialClickHandlers = new Dictionary<SpecialGameState, Action<CardRenderer>>
            {
                { SpecialGameState.SelectingOpponentsPokemon, SelectedOpponentPokemon },
                { SpecialGameState.SelectingOpponentsBenchedPokemon, SelectedOpponentBenchedPokemon },
                { SpecialGameState.AttachingEnergyToBenchedPokemon, SelectedBenchedPokemonForEnergy },
                { SpecialGameState.DiscardingCards, ToggleCardSelected },
                { SpecialGameState.SelectingYourBenchedPokemon, SelectedPlayerBenchedPokemon },
                { SpecialGameState.AttachingEnergyToPokemon, OnTryAttachEnergy }
            };
        }

        private void OnTryAttachEnergy(CardRenderer cardRenderer)
        {
            PokemonCard pokemon = cardRenderer.card as PokemonCard;

            if (pokemon == null)
            {
                return;
            }

            NetworkManager.Instance.gameService.AttachEnergy(pokemon.Id, currentEnergyCard.Id);
            currentEnergyCard = null;
            SpecialState = SpecialGameState.None;
            infoText.text = string.Empty;
        }

        private void ToggleCardSelected(CardRenderer clickedCard)
        {
            clickedCard.SetSelected(!clickedCard.isSelected);
        }

        private void SelectedBenchedPokemonForEnergy(CardRenderer selectedCard)
        {
            if (!playerBench.GetComponentsInChildren<CardRenderer>().Any(controller => controller.card.Id.Equals(selectedCard.card.Id)))
            {
                return;
            }

            var energyCard = energyCardsToAttach.Dequeue();

            energyPokemonMap.Add(energyCard.Id, selectedCard.card.Id);

            if (energyCardsToAttach.Count == 0)
            {
                var response = new AttachedEnergyDoneMessage(energyPokemonMap);
                NetworkManager.Instance.SendToServer(response, true);
                energyPokemonMap.Clear();
            }
        }

        private void Start()
        {
            myId = NetworkManager.Instance.Me.Id;
            var messageId = NetworkManager.Instance.gameService.HostGame(NetworkManager.Instance.Me.Id);

            NetworkManager.Instance.RegisterCallback(messageId, OnGameHosted);
            NetworkManager.Instance.RegisterCallback(MessageTypes.GameUpdate, OnGameUpdated);
            NetworkManager.Instance.RegisterCallback(MessageTypes.SelectOpponentPokemon, OnStartSelectingOpponentPokemon);
            NetworkManager.Instance.RegisterCallback(MessageTypes.SelectFromOpponentBench, OnStartSelectingOpponentBench);
            NetworkManager.Instance.RegisterCallback(MessageTypes.SelectFromYourBench, OnStartSelectingYourBench);
            NetworkManager.Instance.RegisterCallback(MessageTypes.SelectColor, OnStartSelectColor);
            NetworkManager.Instance.RegisterCallback(MessageTypes.PickFromList, OnStartPickFromList);
            NetworkManager.Instance.RegisterCallback(MessageTypes.AttachEnergyToBench, OnStartAttachingEnergyBench);
            NetworkManager.Instance.RegisterCallback(MessageTypes.DeckSearch, OnDeckSearch);
            NetworkManager.Instance.RegisterCallback(MessageTypes.DiscardCards, OnStartDiscardCards);
            NetworkManager.Instance.RegisterCallback(MessageTypes.SelectPriceCards, OnStartPickFromList);
        }

        private void OnStartDiscardCards(object message, NetworkId messageId)
        {
            selectedCards.Clear();
            SpecialState = SpecialGameState.DiscardingCards;
            minSelectedCardCount = ((DiscardCardsMessage)message).Count;

            doneButton.SetActive(true);

            infoText.text = $"Discard {minSelectedCardCount} cards";
        }

        private void OnDeckSearch(object message, NetworkId messageId)
        {
            selectFromListPanel.SetActive(true);
            selectFromListPanel.GetComponent<SelectFromListPanel>().Init((DeckSearchMessage)message);
        }

        private void OnStartAttachingEnergyBench(object message, NetworkId messageId)
        {
            energyCardsToAttach.Clear();
            doneButton.SetActive(true);
            SpecialState = SpecialGameState.AttachingEnergyToBenchedPokemon;

            var realMessage = (AttachEnergyCardsToBenchMessage)message;

            foreach (var card in realMessage.EnergyCards)
            {
                energyCardsToAttach.Enqueue(card);
            }

            infoText.text = $"Select one of your benched pokemon to attach {realMessage.EnergyCards.First().GetName()} to";
        }

        private void OnStartSelectAttachedEnergy(object message, NetworkId messageId)
        {
            infoText.text = "Select one of your pokemon to move a Fire energy from";

            selectFromListPanel.SetActive(true);
            selectFromListPanel.GetComponent<SelectFromListPanel>().Init((PickFromListMessage)message);
        }

        private void OnStartPickFromList(object message, NetworkId messageId)
        {
            selectFromListPanel.SetActive(true);
            selectFromListPanel.GetComponent<SelectFromListPanel>().Init((PickFromListMessage)message);
        }

        private void OnStartSelectColor(object message, NetworkId messageId)
        {
            selectColorPanel.SetActive(true);
            SpecialState = SpecialGameState.SelectingColor;
        }

        private void OnStartSelectingYourBench(object message, NetworkId messageId)
        {
            selectedCards.Clear();

            var maxCount = ((SelectFromYourBench)message).MaxCount;
            var minCount = ((SelectFromYourBench)message).MinCount;
            minSelectedCardCount = minCount;
            SpecialState = SpecialGameState.SelectingYourBenchedPokemon;
            doneButton.SetActive(true);

            var countString = maxCount == minCount ? maxCount.ToString() : "up to " + maxCount;
            infoText.text = $"Select {countString} of your benched pokemon";
        }

        private void OnStartSelectingOpponentBench(object message, NetworkId messageId)
        {
            selectedCards.Clear();
            var maxCount = ((SelectFromOpponentBench)message).MaxCount;
            var minCount = ((SelectFromOpponentBench)message).MinCount;
            minSelectedCardCount = minCount;
            SpecialState = SpecialGameState.SelectingOpponentsBenchedPokemon;
            doneButton.SetActive(true);

            var countString = maxCount == minCount ? maxCount.ToString() : "up to " + maxCount;
            infoText.text = $"Select {countString} of your oppoents benched pokemon";
        }

        private void OnStartSelectingOpponentPokemon(object message, NetworkId messageId)
        {
            selectedCards.Clear();

            var maxCount = ((SelectOpponentPokemon)message).MaxCount;
            var minCount = ((SelectOpponentPokemon)message).MinCount;
            minSelectedCardCount = minCount;

            SpecialState = SpecialGameState.SelectingOpponentsPokemon;
            doneButton.SetActive(true);

            var countString = maxCount == minCount ? maxCount.ToString() : "up to " + maxCount;
            infoText.text = $"Select {countString} of your oppoents pokemon";
        }

        public void OnCardClicked(CardRenderer cardController)
        {
            if (!IsMyTurn)
            {
                return;
            }

            Debug.Log("Clicked: " + cardController.name);

            if (onSpecialClickHandlers.ContainsKey(SpecialState))
            {
                onSpecialClickHandlers[SpecialState].Invoke(cardController);
            }
            else if (onClickHandlers.ContainsKey(gameField.GameState))
            {
                onClickHandlers[gameField.GameState].Invoke(cardController);
            }
            else if (cardController.card is EnergyCard)
            {
                StartAttachingEnergy((EnergyCard)cardController.card);
            }
            else
            {
                cardController.DisplayPopup();
            }
        }

        private void SelectedOpponentPokemon(CardRenderer cardController)
        {
            if (cardController.card.Owner.Id.Equals(myId))
            {
                return;
            }

            cardController.SetSelected(true);
            selectedCards.Add(cardController.card);
        }

        private void SelectedOpponentBenchedPokemon(CardRenderer cardController)
        {
            if (!opponentBench.GetComponentsInChildren<CardRenderer>().Any(controller => controller.card.Id.Equals(cardController.card.Id)))
            {
                return;
            }

            cardController.SetSelected(!cardController.isSelected);
            selectedCards.Add(cardController.card);
        }

        private void SelectedPlayerBenchedPokemon(CardRenderer cardController)
        {
            if (!playerBench.GetComponentsInChildren<CardRenderer>().Any(controller => controller.card.Id.Equals(cardController.card.Id)))
            {
                return;
            }

            cardController.SetSelected(!cardController.isSelected);
            selectedCards.Add(cardController.card);
        }

        private void BothSelectingBenchClicked(CardRenderer cardController)
        {
            cardController.SetSelected(!cardController.isSelected);
            selectedCards.Add(cardController.card);
        }

        private void SetActiveStateClicked(CardRenderer cardController)
        {
            var id = NetworkManager.Instance.gameService.SetActivePokemon(myId, cardController.card.Id);
            NetworkManager.Instance.RegisterCallback(id, OnGameUpdated);
        }

        public void ActivateAbility(Ability ability)
        {
            if (!gameField.ActivePlayer.Id.Equals(myId) || !ability.CanActivate() || ability.PokemonOwner.Owner.Hand.Contains(ability.PokemonOwner))
            {
                return;
            }

            NetworkManager.Instance.gameService.ActivateAbility(ability.Id);
        }

        public void Attack(Attack attack)
        {
            if (!gameField.ActivePlayer.Id.Equals(myId) || !gameField.ActivePlayer.ActivePokemonCard.Attacks.Contains(attack) || !attack.CanBeUsed(gameField, gameField.ActivePlayer, gameField.NonActivePlayer))
            {
                return;
            }

            NetworkManager.Instance.gameService.Attack(attack.Id);
        }

        public void EndTurnButtonClicked()
        {
            NetworkManager.Instance.gameService.EndTurn();
        }

        public void DoneButtonClicked()
        {
            if (gameField.GameState == GameFieldState.BothSelectingBench)
            {
                NetworkManager.Instance.gameService.AddToBench(myId, selectedCards.OfType<PokemonCard>().Select(card => card.Id).ToList());
                selectedCards.Clear();
            }
            else if (SpecialState == SpecialGameState.SelectingOpponentsPokemon)
            {
                if (minSelectedCardCount < selectedCards.Count)
                {
                    return;
                }

                var message = new CardListMessage(selectedCards).ToNetworkMessage(myId);
                message.ResponseTo = NetworkManager.Instance.RespondingTo;
                NetworkManager.Instance.Me.Send(message);
                SpecialState = SpecialGameState.None;
                infoText.text = string.Empty;
            }
            else if (SpecialState == SpecialGameState.SelectingOpponentsBenchedPokemon
            || SpecialState == SpecialGameState.SelectingYourBenchedPokemon
            || SpecialState == SpecialGameState.DiscardingCards)
            {
                if (minSelectedCardCount < selectedCards.Count)
                {
                    return;
                }

                var message = new CardListMessage(selectedCards).ToNetworkMessage(myId);
                message.ResponseTo = NetworkManager.Instance.RespondingTo;
                NetworkManager.Instance.Me.Send(message);
                SpecialState = SpecialGameState.None;
                infoText.text = string.Empty;
            }

            NetworkManager.Instance.RespondingTo = null;
        }

        private void OnGameUpdated(object message)
        {
            OnGameUpdated(message, null);
        }

        private void OnGameUpdated(object message, NetworkId messageId)
        {
            var gameMessage = message is GameFieldMessage ? ((GameFieldMessage)message).Game : (GameField)message;
            Debug.Log("Game updated, handling message");

            GameFieldState oldState = gameField.GameState;

            gameField = gameMessage;
            IsMyTurn = gameField.ActivePlayer.Id.Equals(myId);

            switch (gameField.GameState)
            {
                case GameFieldState.WaitingForConnection:
                    infoText.text = "Waiting for opponent to connect";
                    break;
                case GameFieldState.WaitingForRegistration:
                    infoText.text = "Waiting for opponent to register";
                    break;
                case GameFieldState.BothSelectingActive:
                    infoText.text = "Select your active pokemon";
                    break;
                case GameFieldState.BothSelectingBench:
                    infoText.text = "Select pokemon to add to your bench";
                    break;
                case GameFieldState.InTurn:
                    infoText.text = IsMyTurn ? "Your turn!" : "Opponents turn!";
                    break;
                default:
                    infoText.text = string.Empty;
                    break;
            }

            doneButton.SetActive(statesWithDoneAction.Contains(gameField.GameState));
            endTurnButton.SetActive(IsMyTurn);

            if (gameField.GameState == GameFieldState.WaitingForConnection)
            {
                return;
            }

            Player me = gameField.Players.First(p => p.Id.Equals(myId));
            Player opponent = gameField.Players.First(p => !p.Id.Equals(myId));

            playerHand.SetHand(me.Hand);

            SetActivePokemon(playerActivePokemon, me.ActivePokemonCard, ZoomMode.Center);
            SetActivePokemon(opponentActivePokemon, opponent.ActivePokemonCard, ZoomMode.FromTop);

            SetBenchedPokemon(playerBench, me.BenchedPokemon, ZoomMode.FromBottom);
            SetBenchedPokemon(opponentBench, opponent.BenchedPokemon, ZoomMode.FromTop);

            if (gameStateInfo.ContainsKey(gameField.GameState))
            {
                infoText.text = gameStateInfo[gameField.GameState];
            }
            else
            {
                infoText.text = string.Empty;
            }
        }

        private void SetBenchedPokemon(GameObject parent, IEnumerable<PokemonCard> pokemons, ZoomMode zoomMode)
        {
            parent.DestroyAllChildren();

            foreach (var pokemon in pokemons)
            {
                var spawnedCard = Instantiate(cardPrefab, parent.transform);

                var controller = spawnedCard.GetComponentInChildren<CardRenderer>();
                controller.SetCard(pokemon, zoomMode);
                controller.SetIsBenched();
            }
        }

        private void SetActivePokemon(GameObject parent, PokemonCard pokemonCard, ZoomMode zoomMode)
        {
            if (pokemonCard == null)
            {
                return;
            }

            parent.DestroyAllChildren();

            var spawnedCard = Instantiate(cardPrefab, parent.transform);

            var controller = spawnedCard.GetComponentInChildren<CardRenderer>();
            controller.SetCard(pokemonCard, zoomMode);
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

            if (SpecialState != SpecialGameState.None)
            {
                return;
            }

            switch (gameField.GameState)
            {
                case GameFieldState.WaitingForConnection:
                    infoText.text = "Waiting for opponent to connect";
                    break;
                case GameFieldState.WaitingForRegistration:
                    infoText.text = "Waiting for opponent to register";
                    break;
                case GameFieldState.BothSelectingActive:
                    infoText.text = "Select your active pokemon";
                    break;
                case GameFieldState.BothSelectingBench:
                    infoText.text = "Select pokemon to add to your bench";
                    break;
                case GameFieldState.InTurn:
                    infoText.text = IsMyTurn ? "Your turn!" : "Opponents turn!";
                    break;
                default:
                    infoText.text = string.Empty;
                    break;
            }
        }

        IEnumerator LoadSprite(Card card, Image target)
        {
            string fullCardPath = Path.Combine(Application.streamingAssetsPath, card.GetLogicalName()) + ".png";
            string finalPath = "file:///" + fullCardPath;

            using (var request = UnityWebRequestTexture.GetTexture(finalPath))
            {
                yield return request.SendWebRequest();

                if (request.isNetworkError || request.isHttpError)
                {
                    Debug.LogError("Error fetching texture");
                }

                var texture = DownloadHandlerTexture.GetContent(request);
                target.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }

        internal void OnEnergyTypeClicked(EnergyTypes energyType)
        {
            if (SpecialState == SpecialGameState.SelectingColor)
            {
                NetworkManager.Instance.SendToServer(new SelectColorMessage(energyType), true);
                selectColorPanel.SetActive(false);
            }
        }

        public Player Player
        {
            get
            {
                return gameField?.Players?.FirstOrDefault(player => player.Id.Equals(myId));
            }
        }

        public Player OpponentPlayer
        {
            get
            {
                return gameField?.Players?.FirstOrDefault(player => !player.Id.Equals(myId));
            }
        }
    }
}
