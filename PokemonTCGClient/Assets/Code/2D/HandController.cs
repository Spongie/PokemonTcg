using System.Collections.Generic;
using System.Linq;
using TCGCards;
using UnityEngine;

namespace Assets.Code._2D
{
    public class HandController : MonoBehaviour
    {
        public GameObject cardPrefab;
        public GameObject cardZone;
        public bool isPlayerHand;

        public void SetHand(IEnumerable<Card> cards)
        {
            for (int i = 0; i < cardZone.transform.childCount; i++)
            {
                Destroy(cardZone.transform.GetChild(i).gameObject);
            }

            int index = 0;
            foreach (var card in cards)
            {
                card.IsRevealed = isPlayerHand;
                var spawnedCard = Instantiate(cardPrefab, cardZone.transform);
                spawnedCard.GetComponentInChildren<CardRenderer>().SetCard(card, ZoomMode.FromBottom);
                index++;
            }
        }

        public void FadeOutCards(IEnumerable<Card> cards)
        {
            for (int i = 0; i < cardZone.transform.childCount; i++)
            {
                var cardRenderer = cardZone.transform.GetChild(i).GetComponentInChildren<CardRenderer>();
                CanvasGroup cardCanvas = cardRenderer.GetComponent<CanvasGroup>();
                cardCanvas.alpha = cards.Any(card => card.Id.Equals(cardRenderer.card.Id)) ? 0.2f : 1f;
            }
        }
    }
}
