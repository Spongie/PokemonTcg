using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Code._2D;
using Assets.Code.UI.Events;
using Assets.Code.UI.Game;
using Assets.Code.UI.Game.SpecialStateHandlers;
using Entities;
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
        public GameObject selectAttackPanel;
        public GameObject attackButtonPrefab;
        public BenchController playerBench;
        public BenchController opponentBench;
        public GameObject playerActivePokemon;
        public GameObject opponentActivePokemon;
        public GameObject selectColorPanel;
        public GameObject selectFromListPanel;
        public GameObject cardPrefab;
        public NetworkId myId;
        public GameObject doneButton;
        public GameObject cancelButton;
        public GameObject endTurnButton;
        public GameObject YesButton;
        public GameObject NoButton;
        public GameObject EscapeMenu;
        public GameObject WinMenu;
        public Text winnerText;
        public PilesInfoHandler playerInfoHandler;
        public PilesInfoHandler opponentInfoHandler;

        internal void AddCard(CardRenderer card)
        {
            if (cardRenderers.ContainsKey(card.card.Id))
            {
                cardRenderers[card.card.Id] = card;
                return;
            }

            cardRenderers.Add(card.card.Id, card);
        }

        public Sprite CardBack;

        public bool IsMyTurn;

        public Text infoText;

        private int minSelectedCardCount;
        private int maxSelectedCardCount;
        private List<Card> selectedCards;
        private Dictionary<GameFieldState, Action<CardRenderer>> onClickHandlers;
        private Dictionary<SpecialGameState, Action<CardRenderer>> onSpecialClickHandlers;
        private Dictionary<NetworkId, CardRenderer> cardRenderers;
        private Queue<EnergyCard> energyCardsToAttach;
        private Dictionary<NetworkId, NetworkId> energyPokemonMap;
        private EnergyCard currentEnergyCard;
        private PokemonCard currentEvolvingCard;
        private IDeckFilter currentDeckFilter;
        public List<string> gameLog = new List<string>();
        public ISpecialStateHandler currentClickHandler;

        private void Awake()
        {
            Instance = this;
            selectedCards = new List<Card>();
            energyCardsToAttach = new Queue<EnergyCard>();
            energyPokemonMap = new Dictionary<NetworkId, NetworkId>();
            cardRenderers = new Dictionary<NetworkId, CardRenderer>();
            RegisterClickHandlers();
        }

        internal void StartAttachingEnergy(EnergyCard card)
        {
            SpecialState = SpecialGameState.AttachingEnergyToPokemon;
            currentEnergyCard = card;
            infoText.text = "Select a Pokémon to attach your energy to";
            EnableButtons();
        }

        internal void StartEvolving(PokemonCard card)
        {
            SpecialState = SpecialGameState.SelectPokemonToEvolveOn;
            currentEvolvingCard = card;
            infoText.text = "Select a Pokémon to evolve";
            EnableButtons();
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
                { SpecialGameState.DiscardingCards, OnDiscardCardSelected },
                { SpecialGameState.SelectingAttack, DoNothing },
                { SpecialGameState.SelectingYourBenchedPokemon, SelectedPlayerBenchedPokemon },
                { SpecialGameState.AttachingEnergyToPokemon, OnTryAttachEnergy },
                { SpecialGameState.SelectPokemonToEvolveOn, TryEvolvePokemon },
                { SpecialGameState.SelectPokemonMatchingFilter, OnSelectPokemonWithFilter },
                { SpecialGameState.SelectingRetreatTarget, OnRetreatTargetSelected },
            };
        }

        private void OnDiscardCardSelected(CardRenderer card)
        {
            if (currentDeckFilter != null && !currentDeckFilter.IsCardValid(card.card) || !Player.Hand.Contains(card.card))
            {
                return;
            }

            ToggleCardSelected(card);

            if (minSelectedCardCount == 1 && maxSelectedCardCount == 1)
            {
                DoneButtonClicked();
            }
        }

        private void DoNothing(CardRenderer obj)
        {
            //Yes this is on purpose..   
        }

        private void OnRetreatTargetSelected(CardRenderer clickedCard)
        {
            if (clickedCard.card.Id.Equals(Player.ActivePokemonCard.Id))
            {
                return;
            }

            SpecialState = SpecialGameState.None;
            NetworkManager.Instance.gameService.RetreatPokemon(gameField.Id, clickedCard.card.Id, selectedCards.Select(card => card.Id).ToList());
            selectedCards.Clear();
            EnableButtons();
        }

        internal void StartRetreating(PokemonCard pokemonCard)
        {
            var totalAttachedEnergy = pokemonCard.AttachedEnergy.Sum(energy => energy.GetEnergry().Amount);

            if (pokemonCard.RetreatCost == 0)
            {
                infoText.text = "Select new active Pokémon";
                SpecialState = SpecialGameState.SelectingRetreatTarget;
                selectedCards.Clear();
            }
            else if (totalAttachedEnergy == pokemonCard.RetreatCost)
            {
                infoText.text = "Select new active Pokémon";
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

            EnableButtons();
        }

        internal void OnCardPicked()
        {
            SpecialState = SpecialGameState.None;
        }

        internal void SelectedEnergyForRetreat(List<Card> payedEnergy)
        {
            infoText.text = "Select new active Pokémon";
            SpecialState = SpecialGameState.SelectingRetreatTarget;
            selectedCards.Clear();
            selectedCards.AddRange(payedEnergy);
            EnableButtons();
        }

        private void OnSelectPokemonWithFilter(CardRenderer clickedCard)
        {
            if (currentDeckFilter == null || currentDeckFilter.IsCardValid(clickedCard.card))
            {
                if (minSelectedCardCount == 1 && maxSelectedCardCount == 1)
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
            EnableButtons();
        }

        private void OnTryAttachEnergy(CardRenderer cardRenderer)
        {
            PokemonCard pokemon = cardRenderer.card as PokemonCard;

            if (pokemon == null || Player.Hand.Contains(pokemon) || !pokemon.Owner.Id.Equals(myId))
            {
                return;
            }

            NetworkManager.Instance.gameService.AttachEnergy(gameField.Id, pokemon.Id, currentEnergyCard.Id);
            currentEnergyCard = null;
            SpecialState = SpecialGameState.None;
            infoText.text = string.Empty;
            EnableButtons();
        }

        public void OnCancelClick()
        {
            SpecialState = SpecialGameState.None;
            selectFromListPanel.SetActive(false);

            if (IsMyTurn)
            {
                infoText.text = "Your turn!";
            }
            else
            {
                infoText.text = "Opponent's turn!";
            }

            foreach (var card in selectedCards)
            {
                ToggleCardSelected(GetCardRendererById(card.Id));
            }

            selectedCards.Clear();
            currentDeckFilter = null;
            currentEnergyCard = null;
            currentEvolvingCard = null;
            EnableButtons();
        }

        private void ToggleCardSelected(CardRenderer clickedCard)
        {
            clickedCard.SetSelected(!clickedCard.isSelected);

            if (clickedCard.isSelected)
            {
                selectedCards.Add(clickedCard.card);
            }
            else
            {
                selectedCards.Remove(clickedCard.card);
            }
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

            NetworkManager.Instance.RegisterCallback(MessageTypes.GameUpdate, OnGameUpdated);
            NetworkManager.Instance.RegisterCallback(MessageTypes.SelectOpponentPokemon, OnStartSelectingOpponentPokemon);
            NetworkManager.Instance.RegisterCallback(MessageTypes.SelectFromOpponentBench, OnStartSelectingOpponentBench);
            NetworkManager.Instance.RegisterCallback(MessageTypes.SelectFromYourBench, OnStartSelectingYourBench);
            NetworkManager.Instance.RegisterCallback(MessageTypes.SelectColor, OnStartSelectColor);
            NetworkManager.Instance.RegisterCallback(MessageTypes.PickFromList, OnStartPickFromList);
            NetworkManager.Instance.RegisterCallback(MessageTypes.AttachEnergyToBench, OnStartAttachingEnergyBench);
            NetworkManager.Instance.RegisterCallback(MessageTypes.DeckSearch, OnDeckSearch);
            NetworkManager.Instance.RegisterCallback(MessageTypes.RevealCardsMessage, OnCardsRevealed);
            NetworkManager.Instance.RegisterCallback(MessageTypes.SelectAttack, OnStartSelectAttack);
            NetworkManager.Instance.RegisterCallback(MessageTypes.DiscardCards, OnStartDiscardCards);
            NetworkManager.Instance.RegisterCallback(MessageTypes.SelectPrizeCards, OnStartPickPrize);
            NetworkManager.Instance.RegisterCallback(MessageTypes.GameLogNewMessages, OnNewLogMessage);
            NetworkManager.Instance.RegisterCallback(MessageTypes.GameLogReload, OnGameLogReload);
            NetworkManager.Instance.RegisterCallback(MessageTypes.YesNoMessage, OnYesNoMessage);
            NetworkManager.Instance.RegisterCallback(MessageTypes.SelectFromYourPokemon, OnBeginSelectYourPokemon);
            NetworkManager.Instance.RegisterCallback(MessageTypes.GameOver, OnGameEnded);
            NetworkManager.Instance.RegisterCallback(MessageTypes.Info, OnInfoReceived);
            NetworkManager.Instance.RegisterCallback(MessageTypes.GameEvent, OnGameEventReceived);

            OnGameUpdated(gameField);
        }

        private void OnCardsRevealed(object message, NetworkId arg2)
        {
            var revealMessage = (RevealCardsMessage)message;

            selectFromListPanel.SetActive(true);
            selectFromListPanel.GetComponent<SelectFromListPanel>().InitView(revealMessage.Cards);
            infoText.text = "Look at the revealed cards!";
            EnableButtons();
        }

        private void OnStartSelectAttack(object message, NetworkId messageId)
        {
            SpecialState = SpecialGameState.SelectingAttack;
            var selectAttackMessage = (SelectAttackMessage)message;

            selectAttackPanel.SetActive(true);
            var parent = selectAttackPanel.GetComponentInChildren<VerticalLayoutGroup>().gameObject;
            EnableButtons();
            foreach (var attack in selectAttackMessage.AvailableAttacks)
            {
                var buttonObject = Instantiate(attackButtonPrefab, parent.transform);
                buttonObject.GetComponentInChildren<Text>().text = attack.Name;
                buttonObject.GetComponent<Button>().onClick.AddListener(() => { OnAttackSelected(attack); });
            }
        }

        public void OnAttackSelected(Attack attack) 
        {
            selectAttackPanel.GetComponentInChildren<VerticalLayoutGroup>().gameObject.DestroyAllChildren();
            selectAttackPanel.SetActive(true);

            SpecialState = SpecialGameState.None;
            NetworkManager.Instance.SendToServer(new AttackMessage(attack), true);
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

            if (gameOverMessage.WinnerId.Equals(gameField.Id))
            {
                infoText.text = "It's a draw!";
            }
            else
            {
                infoText.text = iWon ? "You have won!" : "You have lost!";
            }
            
            EnableButtons();

            WinMenu.SetActive(true);
            winnerText.text = infoText.text;
        }

        private void OnBeginSelectYourPokemon(object message, NetworkId messageId)
        {
            var selectMessage = (SelectFromYourPokemonMessage)message;
            SpecialState = SpecialGameState.SelectPokemonMatchingFilter;
            EnableButtons();
            minSelectedCardCount = 1;
            maxSelectedCardCount = 1;

            if (!string.IsNullOrWhiteSpace(selectMessage.Info))
            {
                infoText.text = selectMessage.Info;
                currentDeckFilter = new PokemonOwnerAndTypeFilter(myId);
            }
            else if (selectMessage.TargetTypes.Any())
            {
                var typeText = string.Join(" ", selectMessage.TargetTypes.Select(type => Enum.GetName(typeof(EnergyTypes), type)));
                infoText.text = $"Select one of your {typeText} Pokémon";
                currentDeckFilter = new PokemonOwnerAndTypeFilter(myId, selectMessage.TargetTypes[0]);
            }
            else
            {
                infoText.text = "Select one of your Pokémon";
                currentDeckFilter = new PokemonOwnerAndTypeFilter(myId);
            }

            if (selectMessage.Filter != null)
            {
                currentDeckFilter = selectMessage.Filter;
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
            var discardMessage = ((DiscardCardsMessage)message);
            selectedCards.Clear();
            SpecialState = SpecialGameState.DiscardingCards;
            minSelectedCardCount = discardMessage.Count;
            maxSelectedCardCount = minSelectedCardCount;
            currentDeckFilter = discardMessage.Filters.FirstOrDefault();

            EnableButtons();

            if (minSelectedCardCount > 1)
            {
                doneButton.SetActive(true);
            }

            if (!string.IsNullOrEmpty(discardMessage.Info))
            {
                infoText.text = discardMessage.Info;
            }
            else
            {
                string cardsText = minSelectedCardCount > 1 ? "cards" : "card";
                infoText.text = $"Discard {minSelectedCardCount} {cardsText} from your hand";
            }
        }

        private void OnGameEventReceived(object arg1, NetworkId arg2)
        {
            var gameEvent = ((EventMessage)arg1).GameEvent;
            GameEventHandler.Instance.EnqueueEvent(gameEvent);
        }

        private void OnDeckSearch(object message, NetworkId messageId)
        {
            var deckSearch = (DeckSearchMessage)message;
            minSelectedCardCount = deckSearch.CardCount;
            maxSelectedCardCount = minSelectedCardCount;
            SpecialState = SpecialGameState.SelectingFromList;
            selectFromListPanel.SetActive(true);
            selectFromListPanel.GetComponent<SelectFromListPanel>().Init(deckSearch);
            EnableButtons();
        }

        private void OnStartAttachingEnergyBench(object message, NetworkId messageId)
        {
            energyCardsToAttach.Clear();
            SpecialState = SpecialGameState.AttachingEnergyToBenchedPokemon;
            EnableButtons();

            var realMessage = (AttachEnergyCardsToBenchMessage)message;

            foreach (var card in realMessage.EnergyCards)
            {
                energyCardsToAttach.Enqueue(card);
            }

            infoText.text = $"Select one of your benched Pokémon to attach {realMessage.EnergyCards.First().GetName()} to";
        }

        private void EnableButtons()
        {
            endTurnButton.SetActive(IsMyTurn);

            switch (SpecialState)
            {
                case SpecialGameState.SelectingOpponentsPokemon:
                    doneButton.SetActive(true);
                    cancelButton.SetActive(false);
                    break;
                case SpecialGameState.SelectingOpponentsBenchedPokemon:
                    doneButton.SetActive(true);
                    cancelButton.SetActive(false);
                    break;
                case SpecialGameState.SelectingYourBenchedPokemon:
                    doneButton.SetActive(true);
                    cancelButton.SetActive(false);
                    break;
                case SpecialGameState.DiscardingCards:
                    doneButton.SetActive(true);
                    cancelButton.SetActive(false);
                    break;
                case SpecialGameState.SelectingColor:
                    doneButton.SetActive(false);
                    cancelButton.SetActive(false);
                    break;
                case SpecialGameState.AttachingEnergyToBenchedPokemon:
                    doneButton.SetActive(true);
                    cancelButton.SetActive(false);
                    break;
                case SpecialGameState.SelectPokemonMatchingFilter:
                    doneButton.SetActive(true);
                    cancelButton.SetActive(false);
                    break;
                case SpecialGameState.AttachingEnergyToPokemon:
                    doneButton.SetActive(false);
                    cancelButton.SetActive(true);
                    break;
                case SpecialGameState.SelectPokemonToEvolveOn:
                    doneButton.SetActive(false);
                    cancelButton.SetActive(true);
                    break;
                case SpecialGameState.SelectingYesNo:
                    doneButton.SetActive(false);
                    cancelButton.SetActive(false);
                    break;
                case SpecialGameState.SelectingRetreatTarget:
                    doneButton.SetActive(false);
                    cancelButton.SetActive(true);
                    break;
                case SpecialGameState.SelectEnergyToRetreat:
                    doneButton.SetActive(false);
                    cancelButton.SetActive(true);
                    break;
                case SpecialGameState.SelectingPrize:
                    doneButton.SetActive(false);
                    cancelButton.SetActive(false);
                    break;
                case SpecialGameState.SelectingFromList:
                    doneButton.SetActive(false);
                    cancelButton.SetActive(false);
                    break;
                case SpecialGameState.SelectingAttack:
                    doneButton.SetActive(false);
                    cancelButton.SetActive(false);
                    break;
                case SpecialGameState.None:
                    doneButton.SetActive(false);
                    cancelButton.SetActive(false);
                    break;
                default:
                    doneButton.SetActive(false);
                    cancelButton.SetActive(false);
                    break;
            }
        }

        private void OnStartPickPrize(object message, NetworkId messageId)
        {
            infoText.text = "Select 1 prize card";
            SpecialState = SpecialGameState.SelectingPrize;
            selectFromListPanel.SetActive(true);
            selectFromListPanel.GetComponent<SelectFromListPanel>().Init((SelectPrizeCardsMessage)message, Player.PrizeCards);
            EnableButtons();
        }

        private void OnStartPickFromList(object message, NetworkId messageId)
        {
            SpecialState = SpecialGameState.SelectingFromList;
            selectFromListPanel.SetActive(true);
            selectFromListPanel.GetComponent<SelectFromListPanel>().Init((PickFromListMessage)message);
            EnableButtons();
        }

        private void OnStartSelectColor(object message, NetworkId messageId)
        {
            selectColorPanel.SetActive(true);
            SpecialState = SpecialGameState.SelectingColor;
        }

        private void OnStartSelectingYourBench(object message, NetworkId messageId)
        {
            selectedCards.Clear();
            var selectYourPokemon = ((SelectFromYourBenchMessage)message);
            var maxCount = selectYourPokemon.MaxCount;
            var minCount = selectYourPokemon.MinCount;
            minSelectedCardCount = minCount;
            maxSelectedCardCount = maxCount;
            SpecialState = SpecialGameState.SelectingYourBenchedPokemon;
            EnableButtons();

            if (!string.IsNullOrEmpty(selectYourPokemon.Info))
            {
                infoText.text = selectYourPokemon.Info;
            }
            else
            {
                var countString = maxCount == minCount ? maxCount.ToString() : "up to " + maxCount;
                infoText.text = $"Select {countString} of your benched Pokémon";
            }
        }

        private void OnStartSelectingOpponentBench(object message, NetworkId messageId)
        {
            selectedCards.Clear();
            var selectOpponentsMessage = ((SelectFromOpponentBenchMessage)message);
            var maxCount = selectOpponentsMessage.MaxCount;
            var minCount = selectOpponentsMessage.MinCount;
            minSelectedCardCount = minCount;
            maxSelectedCardCount = maxCount;
            SpecialState = SpecialGameState.SelectingOpponentsBenchedPokemon;
            EnableButtons();

            if (!string.IsNullOrEmpty(selectOpponentsMessage.Info))
            {
                infoText.text = selectOpponentsMessage.Info;
            }
            else
            {
                var countString = maxCount == minCount ? maxCount.ToString() : "up to " + maxCount;
                infoText.text = $"Select {countString} of your opponents benched Pokémon";
            }
        }

        private void OnStartSelectingOpponentPokemon(object message, NetworkId messageId)
        {
            selectedCards.Clear();
            var selectOpponentMessage = ((SelectOpponentPokemonMessage)message);
            var maxCount = selectOpponentMessage.MaxCount;
            var minCount = selectOpponentMessage.MinCount;
            minSelectedCardCount = minCount;
            maxSelectedCardCount = maxCount;

            SpecialState = SpecialGameState.SelectingOpponentsPokemon;
            EnableButtons();

            if (!string.IsNullOrEmpty(selectOpponentMessage.Info))
            {
                infoText.text = selectOpponentMessage.Info;
            }
            else
            {
                var countString = maxCount == minCount ? maxCount.ToString() : "up to " + maxCount;
                infoText.text = $"Select {countString} of your opponents Pokémon";
            }
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

            ToggleCardSelected(cardController);
        }

        private void SelectedOpponentBenchedPokemon(CardRenderer cardController)
        {
            if (!opponentBench.GetComponentsInChildren<CardRenderer>().Any(controller => controller.card.Id.Equals(cardController.card.Id)) || !(cardController.card is PokemonCard))
            {
                return;
            }

            ToggleCardSelected(cardController);
        }

        private void SelectedPlayerBenchedPokemon(CardRenderer cardController)
        {
            if (!playerBench.GetComponentsInChildren<CardRenderer>().Any(controller => controller.card.Id.Equals(cardController.card.Id)) || !(cardController.card is PokemonCard))
            {
                return;
            }

            ToggleCardSelected(cardController);
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
            NetworkManager.Instance.gameService.SetActivePokemon(gameField.Id, myId, cardController.card.Id);
        }

        public void ActivateAbility(Ability ability)
        {
            if (!gameField.ActivePlayer.Id.Equals(myId) || ability.PokemonOwner.Owner.Hand.Contains(ability.PokemonOwner))
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
            if (SpecialState != SpecialGameState.None)
            {
                return;
            }

            NetworkManager.Instance.gameService.EndTurn(gameField.Id);
        }

        public void DoneButtonClicked()
        {
            if (gameField.GameState == GameFieldState.GameOver)
            {
                EnableButtons();
                SceneManager.LoadScene("MainMenu");
                return;
            }
            if (gameField.GameState == GameFieldState.BothSelectingBench)
            {
                NetworkManager.Instance.gameService.AddToBench(gameField.Id, myId, selectedCards.OfType<PokemonCard>().Select(card => card.Id).ToList());
                selectedCards.Clear();
            }
            else if (SpecialState == SpecialGameState.SelectingFromList)
            {
                var message = new CardListMessage(selectedCards.Select(card => card.Id).ToList());
                NetworkManager.Instance.SendToServer(message, true);
                SpecialState = SpecialGameState.None;
                infoText.text = string.Empty;
            }
            else if (SpecialState == SpecialGameState.SelectingOpponentsPokemon || 
                SpecialState == SpecialGameState.SelectPokemonMatchingFilter)
            {
                if (selectedCards.Count < minSelectedCardCount || selectedCards.Count > maxSelectedCardCount)
                {
                    return;
                }

                foreach (var selectedCard in selectedCards)
                {
                    GetCardRendererById(selectedCard.Id).SetSelected(false);
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
                if (selectedCards.Count < minSelectedCardCount || selectedCards.Count > maxSelectedCardCount)
                {
                    return;
                }

                foreach (var selectedCard in selectedCards)
                {
                    GetCardRendererById(selectedCard.Id).SetSelected(false);
                }

                var message = new CardListMessage(selectedCards.Select(card => card.Id).ToList());
                NetworkManager.Instance.SendToServer(message, true);
                SpecialState = SpecialGameState.None;
                infoText.text = string.Empty;
                selectedCards.Clear();
            }

            NetworkManager.Instance.RespondingTo = null;
            EnableButtons();
            currentDeckFilter = null;
        }

        private void OnGameUpdated(object message)
        {
            OnGameUpdated(message, null);
        }

        public void OnGameUpdated(object message, NetworkId messageId)
        {
            var gameMessage = message is GameFieldMessage ? ((GameFieldMessage)message).Game : (GameField)message;
            Debug.Log("Game updated, handling message");

            GameFieldState oldState = gameField.GameState;

            gameField = gameMessage;

            if (gameField.GameState == GameFieldState.WaitingForConnection)
            {
                return;
            }

            IsMyTurn = gameField.ActivePlayer.Id.Equals(myId);
            SpecialState = SpecialGameState.None;
            selectColorPanel.SetActive(false);
            selectedCards.Clear();
            selectFromListPanel.SetActive(false);

            SetInfoAndEnableButtons();

            Player me = gameField.Players.First(p => p.Id.Equals(myId));
            Player opponent = gameField.Players.First(p => !p.Id.Equals(myId));

            playerHand.SetHand(me.Hand);

            SetActivePokemon(playerActivePokemon, me.ActivePokemonCard);
            SetActivePokemon(opponentActivePokemon, opponent.ActivePokemonCard);

            SetBenchedPokemon(playerBench, me.BenchedPokemon);
            SetBenchedPokemon(opponentBench, opponent.BenchedPokemon);

            EnableButtons();
        }

        private void SetInfoAndEnableButtons()
        {
            if (gameField.GameState == GameFieldState.BothSelectingBench)
            {
                doneButton.SetActive(true);
            }

            endTurnButton.SetActive(IsMyTurn);
        }

        public void OnInfoUpdated(GameFieldInfo gameInfo, string textInfo)
        {
            OpponentPlayer.DiscardPile = gameInfo.Opponent.CardsInDiscard;
            OpponentPlayer.PrizeCards = gameInfo.Opponent.PrizeCards;
            OpponentPlayer.ActivePokemonCard = (PokemonCard)gameInfo.Opponent.ActivePokemon;
            OpponentPlayer.BenchedPokemon = gameInfo.Opponent.BenchedPokemon.OfType<PokemonCard>().ToList();
            opponentInfoHandler.UpdateWithInfo(gameInfo.Opponent);

            if (OpponentPlayer != null && OpponentPlayer.ActivePokemonCard != null)
            {
                SetActivePokemon(opponentActivePokemon, OpponentPlayer.ActivePokemonCard);
            }

            Player.DiscardPile = gameInfo.Me.CardsInDiscard;
            Player.PrizeCards = gameInfo.Me.PrizeCards;
            Player.ActivePokemonCard = (PokemonCard)gameInfo.Me.ActivePokemon;
            Player.BenchedPokemon = gameInfo.Me.BenchedPokemon.OfType<PokemonCard>().ToList();
            playerInfoHandler.UpdateWithInfo(gameInfo.Me);

            if (Player != null && Player.ActivePokemonCard != null)
            {
                SetActivePokemon(playerActivePokemon, Player.ActivePokemonCard);
            }

            IsMyTurn = gameInfo.ActivePlayer.Equals(myId);
            CurrentGameState = gameInfo.CurrentState;
            gameField.GameState = CurrentGameState;

            if (!string.IsNullOrWhiteSpace(textInfo))
            {
                infoText.text = textInfo;
            }
            else
            {
                infoText.text = IsMyTurn ? "Your Turn!" : "Opponents turn";
            }

            SetInfoAndEnableButtons();
        }

        private void SetBenchedPokemon(BenchController bench, IEnumerable<PokemonCard> pokemons)
        {
            int index = 0;
            bench.ClearSlots();

            foreach (var pokemon in pokemons)
            {
                if (pokemon == null)
                {
                    index++;
                    continue;
                }

                var slot = bench.GetSlot(index);
                var spawnedCard = Instantiate(cardPrefab, slot.transform);

                var controller = spawnedCard.GetComponentInChildren<CardRenderer>();
                controller.SetCard(pokemon, true);
                controller.SetIsBenched();
                AddCard(controller);
                index++;
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
            controller.SetCard(pokemonCard, true);
            controller.SetIsActivePokemon();
            AddCard(controller);
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
                    infoText.text = "Select your active Pokémon";
                    break;
                case GameFieldState.BothSelectingBench:
                    infoText.text = "Select Pokémon to add to your bench";
                    break;
                default:
                    break;
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

        public CardRenderer GetCardRendererById(NetworkId cardId)
        {
            return cardRenderers[cardId];
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
