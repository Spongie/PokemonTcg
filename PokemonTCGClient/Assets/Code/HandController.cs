using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TCGCards;
using UnityEngine;

namespace Assets.Code
{
    public class HandController : MonoBehaviour
    {

        public float cardOffset = 1.01f;
        public GameObject HandCardPrefab;
        public bool opponentHand;

        void Update()
        {

        }

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
        }
    }
}