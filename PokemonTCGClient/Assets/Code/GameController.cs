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
        public TextMeshProUGUI playerDeckCountText;
        public TextMeshProUGUI opponentDeckCountText;

        public TextMeshProUGUI playerDiscardCountText;
        public Image playedDiscardArt;
        public TextMeshProUGUI opponentDiscardCountText;
        public Image opponentDiscardArt;

        public bool IsMyTurn;

        public Text infoText;

        private int minSelectedCardCount;
        private List<Card> selectedCards;
        private Dictionary<GameFieldState, string> gameStateInfo;
        private Dictionary<GameFieldState, Action<CardRenderer>> onClickHandlers;
        private Dictionary<SpecialGameState, Action<CardRenderer>> onSpecialClickHandlers;

        private static GameFieldState[] statesWithDoneAction = new[]
        {
            GameFieldState.BothSelectingBench
        };

        private void Awake()
        {
            Instance = this;
            selectedCards = new List<Card>();

            InitGamestateInfo();
            RegisterClickHandlers();
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
                { SpecialGameState.SelectingOpponentsPokemon, SelectedOpponentBenchedPokemon },
                { SpecialGameState.SelectingOpponentsBenchedPokemon, SelectedOpponentBenchedPokemon }
            };
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
            NetworkManager.Instance.RegisterCallback(MessageTypes.SelectEnergyFromPokemon, OnStartSelectAttachedEnergy);
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
            throw new NotImplementedException();
        }

        private void OnStartSelectAttachedEnergy(object message, NetworkId messageId)
        {
            throw new NotImplementedException();
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
            Debug.Log("Clicked: " + cardController.name);

            if (onSpecialClickHandlers.ContainsKey(SpecialState))
            {
                onSpecialClickHandlers[SpecialState].Invoke(cardController);
            }
            else if (onClickHandlers.ContainsKey(gameField.GameState))
            {
                onClickHandlers[gameField.GameState].Invoke(cardController);
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

            cardController.SetSelected(true);
            selectedCards.Add(cardController.card);
        }

        private void BothSelectingBenchClicked(CardRenderer cardController)
        {
            cardController.SetSelected(true);
            selectedCards.Add(cardController.card);
        }

        private void SetActiveStateClicked(CardRenderer cardController)
        {
            var id = NetworkManager.Instance.gameService.SetActivePokemon(myId, (PokemonCard)cardController.card);
            NetworkManager.Instance.RegisterCallback(id, OnGameUpdated);
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

            doneButton.SetActive(statesWithDoneAction.Contains(gameField.GameState));
            endTurnButton.SetActive(IsMyTurn);

            if (gameField.GameState == GameFieldState.WaitingForConnection)
            {
                return;
            }

            Player me = gameField.Players.First(p => p.Id.Equals(myId));
            Player opponent = gameField.Players.First(p => !p.Id.Equals(myId));

            if (me.DiscardPile != null)
            {
                playerDiscardCountText.text = me.DiscardPile.Count.ToString();
                if (me.DiscardPile.Any())
                {
                    playedDiscardArt.enabled = true;
                    StartCoroutine(LoadSprite(me.DiscardPile.Last(), playedDiscardArt));
                }
                else
                {
                    playedDiscardArt.enabled = false;
                }
            }
            if (opponent.DiscardPile != null)
            {
                opponentDiscardCountText.text = opponent.DiscardPile.Count.ToString();
                if (opponent.DiscardPile.Any())
                {
                    opponentDiscardArt.enabled = true;
                    StartCoroutine(LoadSprite(opponent.DiscardPile.Last(), opponentDiscardArt));
                }
                else
                {
                    opponentDiscardArt.enabled = false;
                }
            }

            if (me.Deck != null)
            {
                playerDeckCountText.text = me.Deck.Cards.Count.ToString();
            }
            if (opponent.Deck != null)
            {
                opponentDeckCountText.text = opponent.Deck.Cards.Count.ToString();
            }

            playerHand.SetHand(me.Hand);

            SetActivePokemon(playerActivePokemon, me.ActivePokemonCard, ZoomMode.Center);
            SetActivePokemon(opponentActivePokemon, opponent.ActivePokemonCard, ZoomMode.FromTop);

            SetBenchedPokemon(playerBench, me.BenchedPokemon, ZoomMode.FromBottom);
            SetBenchedPokemon(opponentBench, opponent.BenchedPokemon, ZoomMode.FromBottom);

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
                default:
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
    }
}
