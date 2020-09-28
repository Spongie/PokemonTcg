using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Code._2D;
using Assets.Code.UI.Game;
using NetworkingCore;
using NetworkingCore.Messages;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Deckfilters;
using TCGCards.Core.GameEvents;
using TCGCards.Core.Messages;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
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
        public GameObject YesButton;
        public GameObject NoButton;
        public GameObject EscapeMenu;
        public GameObject WinMenu;
        public Text winnerText;
        public EventLogViewer eventViewer;

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
        private PokemonCard currentEvolvingCard;
        private IDeckFilter currentDeckFilter;

        public List<string> gameLog = new List<string>();

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

        internal void StartEvolving(PokemonCard card)
        {
            SpecialState = SpecialGameState.SelectPokemonToEvolveOn;
            currentEvolvingCard = card;
            infoText.text = "Select a pokemon to evolve";
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
                //TODO: SelectAttackMessage
                { SpecialGameState.SelectingOpponentsPokemon, SelectedOpponentPokemon },
                { SpecialGameState.SelectingOpponentsBenchedPokemon, SelectedOpponentBenchedPokemon },
                { SpecialGameState.AttachingEnergyToBenchedPokemon, SelectedBenchedPokemonForEnergy },
                { SpecialGameState.DiscardingCards, ToggleCardSelected },
                { SpecialGameState.SelectingYourBenchedPokemon, SelectedPlayerBenchedPokemon },
                { SpecialGameState.AttachingEnergyToPokemon, OnTryAttachEnergy },
                { SpecialGameState.SelectPokemonToEvolveOn, TryEvolvePokemon },
                { SpecialGameState.SelectPokemonMatchingFilter, OnSelectPokemonWithFilter },
                { SpecialGameState.SelectingRetreatTarget, OnRetreatTargetSelected },
            };
        }

        private void OnRetreatTargetSelected(CardRenderer clickedCard)
        {
            SpecialState = SpecialGameState.None;
            NetworkManager.Instance.gameService.RetreatPokemon(gameField.Id, clickedCard.card.Id, selectedCards.Select(card => card.Id).ToList());
            selectedCards.Clear();
        }

        internal void StartRetreating(PokemonCard pokemonCard)
        {
            var totalAttachedEnergy = pokemonCard.AttachedEnergy.Sum(energy => energy.GetEnergry().Amount);

            if (totalAttachedEnergy == pokemonCard.RetreatCost)
            {
                infoText.text = "Select new active pokemon";
                SpecialState = SpecialGameState.SelectingRetreatTarget;
                selectedCards.Clear();
                selectedCards.AddRange(pokemonCard.AttachedEnergy);
            }
            else
            {
                infoText.text = "Pick energy to use for retreat";
                SpecialState = SpecialGameState.SelectEnergyToRetreat;
                selectedCards.Clear();
                selectFromListPanel.SetActive(true);
                selectFromListPanel.GetComponent<SelectFromListPanel>().InitEnergyCountSelect(pokemonCard.AttachedEnergy, pokemonCard.RetreatCost);
            }
        }

        internal void OnCardPicked()
        {
            SpecialState = SpecialGameState.None;
        }

        internal void SelectedEnergyForRetreat(List<Card> selectedCards)
        {
            infoText.text = "Select new active pokemon";
            SpecialState = SpecialGameState.SelectingRetreatTarget;
            selectedCards.Clear();
            selectedCards.AddRange(selectedCards);
        }

        private void OnSelectPokemonWithFilter(CardRenderer clickedCard)
        {
            if (currentDeckFilter == null || currentDeckFilter.IsCardValid(clickedCard.card))
            {
                if (minSelectedCardCount == 1)
                {
                    var message = new CardListMessage(new[] { clickedCard.card.Id }.ToList());
                    NetworkManager.Instance.SendToServer(message, true);
                    SpecialState = SpecialGameState.None;
                    infoText.text = string.Empty;

                    return;
                }

                ToggleCardSelected(clickedCard);
            }
        }

        private void TryEvolvePokemon(CardRenderer cardRenderer)
        {
            PokemonCard pokemon = cardRenderer.card as PokemonCard;

            if (pokemon == null)
            {
                return;
            }

            NetworkManager.Instance.gameService.EvolvePokemon(gameField.Id, pokemon.Id, currentEvolvingCard.Id);
            currentEvolvingCard = null;
            SpecialState = SpecialGameState.None;
            infoText.text = string.Empty;
        }

        private void OnTryAttachEnergy(CardRenderer cardRenderer)
        {
            PokemonCard pokemon = cardRenderer.card as PokemonCard;

            if (pokemon == null)
            {
                return;
            }

            NetworkManager.Instance.gameService.AttachEnergy(gameField.Id, pokemon.Id, currentEnergyCard.Id);
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
            gameField = NetworkManager.Instance.CurrentGame;

            //TODO Send more info messages
            NetworkManager.Instance.RegisterCallback(MessageTypes.GameUpdate, OnGameUpdated);
            NetworkManager.Instance.RegisterCallback(MessageTypes.SelectOpponentPokemon, OnStartSelectingOpponentPokemon);
            NetworkManager.Instance.RegisterCallback(MessageTypes.SelectFromOpponentBench, OnStartSelectingOpponentBench);
            NetworkManager.Instance.RegisterCallback(MessageTypes.SelectFromYourBench, OnStartSelectingYourBench);
            NetworkManager.Instance.RegisterCallback(MessageTypes.SelectColor, OnStartSelectColor);
            NetworkManager.Instance.RegisterCallback(MessageTypes.PickFromList, OnStartPickFromList);
            NetworkManager.Instance.RegisterCallback(MessageTypes.AttachEnergyToBench, OnStartAttachingEnergyBench);
            NetworkManager.Instance.RegisterCallback(MessageTypes.DeckSearch, OnDeckSearch);
            NetworkManager.Instance.RegisterCallback(MessageTypes.DiscardCards, OnStartDiscardCards);
            NetworkManager.Instance.RegisterCallback(MessageTypes.SelectPriceCards, OnStartPickPrize);
            NetworkManager.Instance.RegisterCallback(MessageTypes.GameLogNewMessages, OnNewLogMessage);
            NetworkManager.Instance.RegisterCallback(MessageTypes.GameLogReload, OnGameLogReload);
            NetworkManager.Instance.RegisterCallback(MessageTypes.YesNoMessage, OnYesNoMessage);
            NetworkManager.Instance.RegisterCallback(MessageTypes.SelectFromYourPokemon, OnBeginSelectYourPokemon);
            NetworkManager.Instance.RegisterCallback(MessageTypes.GameOver, OnGameEnded);
            NetworkManager.Instance.RegisterCallback(MessageTypes.Info, OnInfoReceived);
            NetworkManager.Instance.RegisterCallback(MessageTypes.GameEvent, OnGameEventReceived);
        }

        private void OnDestroy()
        {
            DeRegisterCallbacks();
        }

        private void DeRegisterCallbacks()
        {
            foreach (MessageTypes messageType in Enum.GetValues(typeof(MessageTypes)))
            {
                NetworkManager.Instance.DeRegisterCallback(messageType);
            }
        }

        private void OnInfoReceived(object message, NetworkId messageId)
        {
            infoText.text = ((InfoMessage)message).Info;
        }

        private void OnGameEnded(object message, NetworkId messageId)
        {
            var gameOverMessage = (GameOverMessage)message;
            bool iWon = gameOverMessage.WinnerId.Equals(myId);
            gameField.GameState = GameFieldState.GameOver;
            infoText.text = iWon ? "You have won" : "You have lost";
            doneButton.SetActive(true);

            WinMenu.SetActive(true);
            winnerText.text = infoText.text;
        }

        private void OnBeginSelectYourPokemon(object message, NetworkId messageId)
        {
            var selectMessage = (SelectFromYourPokemonMessage)message;
            SpecialState = SpecialGameState.SelectPokemonMatchingFilter;
            doneButton.SetActive(true);
            minSelectedCardCount = 1;

            if (!string.IsNullOrWhiteSpace(selectMessage.Info))
            {
                infoText.text = selectMessage.Info;
            }
            if (selectMessage.TargetTypes.Any())
            {
                var typeText = string.Join(" ", selectMessage.TargetTypes.Select(type => Enum.GetName(typeof(EnergyTypes), type)));
                infoText.text = $"Select one of your {typeText} pokemon";
                currentDeckFilter = new PokemonOfTypeFilter(selectMessage.TargetTypes);
            }
            else
            {
                infoText.text = "Select one of your pokemon";
            }
        }

        private void OnYesNoMessage(object message, NetworkId messageId)
        {
            infoText.text = ((YesNoMessage)message).Message;
            SpecialState = SpecialGameState.SelectingYesNo;
            YesButton.SetActive(true);
            NoButton.SetActive(true);
        }

        public void OnYesClicked()
        {
            SpecialState = SpecialGameState.None;
            NetworkManager.Instance.SendToServer(new YesNoMessage { AnsweredYes = true }, true);
            YesButton.SetActive(false);
            NoButton.SetActive(false);
        }

        public void OnNoClicked()
        {
            SpecialState = SpecialGameState.None;
            NetworkManager.Instance.SendToServer(new YesNoMessage { AnsweredYes = false }, true);
            YesButton.SetActive(false);
            NoButton.SetActive(false);
        }

        private void OnGameLogReload(object obj, NetworkId messageId)
        {
            var message = (GameLogReloadMessage)obj;
            gameLog = message.GameLog.Messages;
        }

        private void OnNewLogMessage(object obj, NetworkId messageId)
        {
            var message = (GameLogAddMessage)obj;

            gameLog.AddRange(message.NewMessages);
        }

        private void OnStartDiscardCards(object message, NetworkId messageId)
        {
            selectedCards.Clear();
            SpecialState = SpecialGameState.DiscardingCards;
            minSelectedCardCount = ((DiscardCardsMessage)message).Count;

            doneButton.SetActive(true);

            infoText.text = $"Discard {minSelectedCardCount} cards";
        }

        private void OnGameEventReceived(object arg1, NetworkId arg2)
        {
            var card = ((EventMessage)arg1).GameEvent.GetCardToDisplay();

            if (card == null)
            {
                return;
            }

            eventViewer?.QueueEvent(card);
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

        private void OnStartPickPrize(object message, NetworkId messageId)
        {
            SpecialState = SpecialGameState.SelectingPrize;
            selectFromListPanel.SetActive(true);
            selectFromListPanel.GetComponent<SelectFromListPanel>().Init((SelectPriceCardsMessage)message, Player.PrizeCards);
        }

        private void OnStartPickFromList(object message, NetworkId messageId)
        {
            SpecialState = SpecialGameState.SelectingFromList;
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

            var maxCount = ((SelectFromYourBenchMessage)message).MaxCount;
            var minCount = ((SelectFromYourBenchMessage)message).MinCount;
            minSelectedCardCount = minCount;
            SpecialState = SpecialGameState.SelectingYourBenchedPokemon;
            doneButton.SetActive(true);

            var countString = maxCount == minCount ? maxCount.ToString() : "up to " + maxCount;
            infoText.text = $"Select {countString} of your benched pokemon";
        }

        private void OnStartSelectingOpponentBench(object message, NetworkId messageId)
        {
            selectedCards.Clear();
            var maxCount = ((SelectFromOpponentBenchMessage)message).MaxCount;
            var minCount = ((SelectFromOpponentBenchMessage)message).MinCount;
            minSelectedCardCount = minCount;
            SpecialState = SpecialGameState.SelectingOpponentsBenchedPokemon;
            doneButton.SetActive(true);

            var countString = maxCount == minCount ? maxCount.ToString() : "up to " + maxCount;
            infoText.text = $"Select {countString} of your oppoents benched pokemon";
        }

        private void OnStartSelectingOpponentPokemon(object message, NetworkId messageId)
        {
            selectedCards.Clear();

            var maxCount = ((SelectOpponentPokemonMessage)message).MaxCount;
            var minCount = ((SelectOpponentPokemonMessage)message).MinCount;
            minSelectedCardCount = minCount;

            SpecialState = SpecialGameState.SelectingOpponentsPokemon;
            doneButton.SetActive(true);

            var countString = maxCount == minCount ? maxCount.ToString() : "up to " + maxCount;
            infoText.text = $"Select {countString} of your opponents pokemon";
        }

        public void OnCardClicked(CardRenderer cardController)
        {
            Debug.Log("Clicked: " + cardController.name);

            var state = gameField == null ? GameFieldState.InTurn : gameField.GameState;

            if (onSpecialClickHandlers.ContainsKey(SpecialState))
            {
                onSpecialClickHandlers[SpecialState].Invoke(cardController);
                return;
            }
            else if (onClickHandlers.ContainsKey(state))
            {
                onClickHandlers[state].Invoke(cardController);
                return;
            }

            if (!IsMyTurn)
            {
                return;
            }

            if (cardController.card is EnergyCard)
            {
                StartAttachingEnergy((EnergyCard)cardController.card);
            }
            else if (cardController.card is TrainerCard)
            {
                NetworkManager.Instance.gameService.PlayCard(gameField.Id, cardController.card.Id);
            }
            else
            {
                cardController.DisplayPopup();
            }
        }

        private void SelectedOpponentPokemon(CardRenderer cardController)
        {
            if (cardController.card.Owner.Id.Equals(myId) || !(cardController.card is PokemonCard))
            {
                return;
            }
            
            cardController.SetSelected(true);
            selectedCards.Add(cardController.card);
        }

        private void SelectedOpponentBenchedPokemon(CardRenderer cardController)
        {
            if (!opponentBench.GetComponentsInChildren<CardRenderer>().Any(controller => controller.card.Id.Equals(cardController.card.Id)) || !(cardController.card is PokemonCard))
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
            if (cardController.isSelected)
            {
                cardController.SetSelected(false);
                selectedCards.Remove(cardController.card);
            }
            else
            {
                cardController.SetSelected(true);
                selectedCards.Add(cardController.card);
            }
        }

        private void SetActiveStateClicked(CardRenderer cardController)
        {
            var id = NetworkManager.Instance.gameService.SetActivePokemon(gameField.Id, myId, cardController.card.Id);
            NetworkManager.Instance.RegisterCallbackById(id, OnGameUpdated);
        }

        public void ActivateAbility(Ability ability)
        {
            if (!gameField.ActivePlayer.Id.Equals(myId) || !ability.CanActivate() || ability.PokemonOwner.Owner.Hand.Contains(ability.PokemonOwner))
            {
                return;
            }

            NetworkManager.Instance.gameService.ActivateAbility(gameField.Id, ability.Id);
        }

        public void Attack(Attack attack)
        {
            if (!gameField.ActivePlayer.Id.Equals(myId) || !gameField.ActivePlayer.ActivePokemonCard.Attacks.Contains(attack) || !attack.CanBeUsed(gameField, gameField.ActivePlayer, gameField.NonActivePlayer))
            {
                return;
            }

            NetworkManager.Instance.gameService.Attack(gameField.Id, attack.Id);
        }

        public void EndTurnButtonClicked()
        {
            NetworkManager.Instance.gameService.EndTurn(gameField.Id);
        }

        public void DoneButtonClicked()
        {
            if (gameField.GameState == GameFieldState.GameOver)
            {
                doneButton.SetActive(true);
                SceneManager.LoadScene("MainMenu");
                return;
            }
            if (gameField.GameState == GameFieldState.BothSelectingBench)
            {
                NetworkManager.Instance.gameService.AddToBench(gameField.Id, myId, selectedCards.OfType<PokemonCard>().Select(card => card.Id).ToList());
                selectedCards.Clear();
            }
            else if (SpecialState == SpecialGameState.SelectingOpponentsPokemon || 
                SpecialState == SpecialGameState.SelectPokemonMatchingFilter)
            {
                if (minSelectedCardCount < selectedCards.Count)
                {
                    return;
                }

                var message = new CardListMessage(selectedCards.Select(card => card.Id).ToList());
                NetworkManager.Instance.SendToServer(message, true);
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

                var message = new CardListMessage(selectedCards.Select(card => card.Id).ToList()).ToNetworkMessage(myId);
                message.ResponseTo = NetworkManager.Instance.RespondingTo;
                NetworkManager.Instance.Me.Send(message);
                SpecialState = SpecialGameState.None;
                infoText.text = string.Empty;
            }

            NetworkManager.Instance.RespondingTo = null;
            doneButton.SetActive(true);
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

            if (gameField.GameState == GameFieldState.BothSelectingBench)
            {
                doneButton.SetActive(true);
            }

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
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EscapeMenu.SetActive(!EscapeMenu.activeSelf);
            }

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

        public void ExitGameClick()
        {
            NetworkManager.Instance.gameService.LeaveGame(myId, gameField.Id);
            SceneManager.LoadScene("MainMenu");
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
