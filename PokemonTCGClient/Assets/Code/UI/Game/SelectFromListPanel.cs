using NetworkingCore;
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
        private List<Card> selectedCards = new List<Card>();
        private HashSet<NetworkId> availableCards;
        private int limit;
        private int minCount;
        public bool onlyView = false;

        private void ClearOldCards()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
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
                    var spawnedCard = Instantiate(previewCard, transform);
                    spawnedCard.GetComponent<CardRenderer>().SetCard(card, ZoomMode.None);
                    availableCards.Add(card.Id);
                }
            }

            foreach (var card in deckSearchMessage.Deck.Cards)
            {
                if (!deckSearchMessage.Filters.All(filter => filter.IsCardValid(card)))
                {
                    var spawnedCard = Instantiate(previewCard, transform);
                    var cardRenderer = spawnedCard.GetComponent<CardRenderer>();
                    cardRenderer.FadeOut();
                    cardRenderer.SetCard(card, ZoomMode.None);
                }
            }
        }

        internal void InitView(List<Card> discardPile)
        {
            ClearOldCards();
            onlyView = true;

            foreach (var card in discardPile)
            {
                var spawnedCard = Instantiate(previewCard, transform);
                spawnedCard.GetComponent<CardRenderer>().SetCard(card, ZoomMode.None);
            }
        }

        public void Init(PickFromListMessage pickFromListMessage)
        {
            ClearOldCards();
            selectedCards.Clear();
            onlyView = false;
            availableCards = new HashSet<NetworkId>();
            limit = pickFromListMessage.MaxCount;
            minCount = pickFromListMessage.MinCount;

            foreach (var card in pickFromListMessage.PossibleChoices)
            {
                var spawnedCard = Instantiate(previewCard, transform);
                spawnedCard.GetComponent<CardRenderer>().SetCard(card, ZoomMode.None);
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

            if (selectedCards.Count < minCount || selectedCards.Count > limit)
            {
                return;
            }

            var message = new CardListMessage(selectedCards.Select(card => card.Id).ToList()).ToNetworkMessage(NetworkManager.Instance.Me.Id);
            message.ResponseTo = NetworkManager.Instance.RespondingTo;
            NetworkManager.Instance.Me.Send(message);

            selectedCards.Clear();
            gameObject.SetActive(false);
        }
    }
}
