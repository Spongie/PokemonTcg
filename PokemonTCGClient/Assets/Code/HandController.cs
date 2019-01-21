using System.Collections;
using System.Collections.Generic;
using TCGCards;
using UnityEngine;

namespace Assets.Code
{
    public class HandController : MonoBehaviour
    {
        public GameObject HandCardPrefab;
        public bool opponentHand;

        public void SetHand(IEnumerable<Card> cards)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            foreach (var card in cards)
            {
                GameObject cardInHand = Instantiate(HandCardPrefab, transform);
                var cardController = cardInHand.GetComponent<CardController>();
                cardController.opponentCard = opponentHand;
                cardController.inHand = true;
                cardController.SetCard(card);
            }

            GetComponent<HandStacker>().ReDrawHand();
        }

        public void FadeInCards(List<Card> cards)
        {
            foreach (var oldCard in transform.GetComponentsInChildren<CardController>())
            {
                if (cards.Contains(oldCard.card))
                {
                    oldCard.FadeIn();
                }
                else
                {
                    oldCard.FadeOut();
                }
            }
        }

        public void FadeInAll()
        {
            foreach (var oldCard in transform.GetComponentsInChildren<CardController>())
            {
                oldCard.FadeIn();
            }
        }
    }
}