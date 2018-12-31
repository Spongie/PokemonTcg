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
            foreach (var oldCard in transform.GetComponentsInChildren<CardController>())
            {
                Destroy(oldCard);
            }

            foreach (var card in cards)
            {
                GameObject cardInHand = Instantiate(HandCardPrefab, transform);
                var cardController = cardInHand.GetComponent<CardController>();
                cardController.card = card;
                cardController.opponentCard = opponentHand;
                cardController.inHand = true;
                cardController.ReloadImage();
            }

            GetComponent<HandStacker>().ReDrawHand();
        }

        public void FadeOut()
        {
            //foreach (var card in GetComponentsInChildren<CardController>())
            //{
            //    StartCoroutine(card.FadeOut());
            //}
        }

        public void FadeIn()
        {
            //foreach (var card in GetComponentsInChildren<CardController>())
            //{
            //    StartCoroutine(card.FadeIn());
            //}
        }
    }
}