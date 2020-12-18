using TCGCards;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code._2D.GameCard
{
    [RequireComponent(typeof(Image))]
    public class Attachment : MonoBehaviour
    {
        private Image image;
        public TrainerCard card;

        private void Start()
        {
            image = GetComponent<Image>();
        }

        internal void SetCard(TrainerCard trainerCard)
        {
            if (card == null || !card.Id.Equals(trainerCard.Id))
            {
                CardImageLoader.Instance.LoadSprite(trainerCard, image);
            }

            card = trainerCard;
        }
    }
}
