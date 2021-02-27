using TCGCards;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code._2D.GameCard
{
    [RequireComponent(typeof(Image))]
    public class Attachment : MonoBehaviour
    {
        public Image image;
        public TrainerCard card;

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
