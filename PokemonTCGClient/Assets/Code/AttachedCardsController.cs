using TCGCards;
using UnityEngine;

namespace Assets.Code
{
    public class AttachedCardsController : MonoBehaviour
    {
        public GameObject CardPrefab;
        private PokemonCard card;

        public void SetCard(Card card)
        {
            this.card = (PokemonCard)card;

            foreach (var energyCard in this.card.AttachedEnergy)
            {
                GameObject energyCardObject = Instantiate(CardPrefab, transform);
                var attachedCard = energyCardObject.GetComponent<AttachedCardController>();
                attachedCard.SetCard(energyCard);
                attachedCard.GetComponent<Canvas>().sortingOrder = 1;
            }

            //TODO: Handle Equipments
        }
    }
}
