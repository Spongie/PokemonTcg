using System.Collections.Generic;
using System.Linq;
using TCGCards;
using UnityEngine;

namespace Assets.Code._2D
{
    public class HandController : MonoBehaviour
    {
        public GameObject cardPrefab;
        public bool isPlayerHand;

        public void SetHand(IEnumerable<Card> cards)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            int index = 0;
            foreach (var card in cards)
            {
                card.IsRevealed = isPlayerHand;
                var spawnedCard = Instantiate(cardPrefab, transform);
                spawnedCard.GetComponentInChildren<CardRenderer>().SetCard(card, ZoomMode.FromBottom);
                index++;
            }
        }

        public void FadeOutCards(IEnumerable<Card> cards)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var cardRenderer = transform.GetChild(i).GetComponentInChildren<CardRenderer>();
                CanvasGroup cardCanvas = cardRenderer.GetComponent<CanvasGroup>();
                cardCanvas.alpha = cards.Any(card => card.Id.Equals(cardRenderer.card.Id)) ? 0.2f : 1f;

            }
        }
    }
}
