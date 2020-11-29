using NetworkingCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core.Messages;
using UnityEngine;

namespace Assets.Code.UI.Game
{
    public class SelectFromListPanel : MonoBehaviour
    {
        public GameObject previewCard;
        public GameObject contentObject;
        private List<Card> selectedCards = new List<Card>();
        private HashSet<NetworkId> availableCards;
        private int limit;
        private int minCount;
        public bool onlyView = false;
        private int energyAmountToSelect;

        private void ClearOldCards()
        {
            for (int i = 0; i < contentObject.transform.childCount; i++)
            {
                var child = contentObject.transform.GetChild(i);
                if (child.tag == "Ignore")
                {
                    continue;
                }

                Destroy(child.gameObject);
            }
        }

        public void Init(DeckSearchMessage deckSearchMessage)
        {
            ClearOldCards();
            selectedCards.Clear();
            onlyView = false;
            availableCards = new HashSet<NetworkId>();
            limit = deckSearchMessage.CardCount;
            minCount = deckSearchMessage.CardCount;

            foreach (var card in deckSearchMessage.Deck.Cards)
            {
                if (deckSearchMessage.Filters.All(filter => filter.IsCardValid(card)))
                {
                    var spawnedCard = Instantiate(previewCard, contentObject.transform);
                    spawnedCard.GetComponent<CardRenderer>().SetCard(card, ZoomMode.None, false, true);
                    availableCards.Add(card.Id);
                }
            }

            foreach (var card in deckSearchMessage.Deck.Cards)
            {
                if (!deckSearchMessage.Filters.All(filter => filter.IsCardValid(card)))
                {
                    var spawnedCard = Instantiate(previewCard, contentObject.transform);
                    var cardRenderer = spawnedCard.GetComponent<CardRenderer>();
                    cardRenderer.FadeOut();
                    cardRenderer.SetCard(card, ZoomMode.None, false, true);
                }
            }
        }

        public void InitEnergyCountSelect(List<EnergyCard> cards, int energyAmountToSelect)
        {
            ClearOldCards();
            selectedCards.Clear();
            onlyView = false;
            availableCards = new HashSet<NetworkId>();
            this.energyAmountToSelect = energyAmountToSelect;

            foreach (var card in cards)
            {
                var spawnedCard = Instantiate(previewCard, contentObject.transform);
                spawnedCard.GetComponent<CardRenderer>().SetCard(card, ZoomMode.None, false, true);
                availableCards.Add(card.Id);
            }
        }

        internal void InitView(List<Card> discardPile)
        {
            ClearOldCards();
            onlyView = true;

            foreach (var card in discardPile)
            {
                var spawnedCard = Instantiate(previewCard, contentObject.transform);
                spawnedCard.GetComponent<CardRenderer>().SetCard(card, ZoomMode.None, false, true);
            }
        }

        public void Init(PickFromListMessage pickFromListMessage)
        {
            InitForCards(pickFromListMessage.PossibleChoices, pickFromListMessage.MinCount, pickFromListMessage.MaxCount);
        }

        private void InitForCards(IEnumerable<Card> cards, int minCount, int maxCount)
        {
            ClearOldCards();
            selectedCards.Clear();
            onlyView = false;
            availableCards = new HashSet<NetworkId>();
            limit = maxCount;
            this.minCount = minCount;

            foreach (var card in cards)
            {
                var spawnedCard = Instantiate(previewCard, contentObject.transform);
                spawnedCard.GetComponent<CardRenderer>().SetCard(card, ZoomMode.None, false, true);
                availableCards.Add(card.Id);
            }
        }

        internal void OnCardClicked(CardRenderer cardRenderer)
        {
            if (onlyView || !availableCards.Contains(cardRenderer.card.Id))
            {
                return;
            }

            if (selectedCards.Any(card => card.Id == cardRenderer.card.Id))
            {
                selectedCards.Remove(cardRenderer.card);
                cardRenderer.SetSelected(false);
            }
            else
            {
                selectedCards.Add(cardRenderer.card);
                cardRenderer.SetSelected(true);
            }
        }

        public void DoneClick()
        {
            if (onlyView)
            {
                selectedCards?.Clear();
                gameObject.SetActive(false);
                return;
            }
            else if (energyAmountToSelect > 0)
            {
                var amountSelected = selectedCards.OfType<EnergyCard>().Sum(card => card.GetEnergry().Amount);

                if (amountSelected < energyAmountToSelect)
                {
                    return;
                }

                GameController.Instance.SelectedEnergyForRetreat(selectedCards);
                selectedCards?.Clear();
                gameObject.SetActive(false);

                return;
            }

            if (selectedCards.Count < minCount || selectedCards.Count > limit)
            {
                return;
            }

            var message = new CardListMessage(selectedCards.Select(card => card.Id).ToList());
            
            NetworkManager.Instance.SendToServer(message, true);

            GameController.Instance.OnCardPicked();

            selectedCards.Clear();
            gameObject.SetActive(false);
        }

        internal void Init(SelectPrizeCardsMessage message, IEnumerable<Card> cards)
        {
            minCount = 1;
            limit = 1;
            InitForCards(cards, message.Amount, message.Amount);
        }
    }
}
