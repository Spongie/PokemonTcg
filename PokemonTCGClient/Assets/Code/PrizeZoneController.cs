using System.Collections.Generic;
using TCGCards;
using UnityEngine;

namespace Assets.Code
{
    public class PrizeZoneController : MonoBehaviour
    {
        public GameObject PrizeCardPrefab;
        public bool opponentZone;

        void Update()
        {

        }

        public void SetPrizeCards(IEnumerable<Card> cards)
        {
            foreach (var oldCard in transform.GetComponentsInChildren<CardController>())
            {
                Destroy(oldCard);
            }

            foreach (var card in cards)
            {
                GameObject cardInHand = Instantiate(PrizeCardPrefab, transform);
                var cardController = cardInHand.GetComponent<CardController>();
                cardController.card = card;
                cardController.opponentCard = opponentZone;
                cardController.inHand = false;
                cardController.ReloadImage();
            }
        }
    }
}
