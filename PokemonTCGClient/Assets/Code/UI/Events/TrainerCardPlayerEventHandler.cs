using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCGCards;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.UI.Events
{
    public class TrainerCardPlayerEventHandler : MonoBehaviour
    {
        public Image cardImage;
        public CardImageLoader loader;

        public void TriggerCardPlayer(TrainerCard card)
        {
            GameController.Instance.playerHand.RemoveCard(card);
            StartCoroutine(DisplayCardEnumerator(card));
        }

        IEnumerator DisplayCardEnumerator(TrainerCard card)
        {
            cardImage.sprite = null;
            cardImage.rectTransform.localScale = new Vector3(0.1f, 0.1f, 1);
            cardImage.color = new Color(cardImage.color.r, cardImage.color.g, cardImage.color.b, 0);
            
            yield return loader.LoadSpriteRoutine(card, cardImage);

            while (true)
            {
                bool anyChanged = false;

                if (cardImage.color.a < 1)
                {
                    cardImage.color = new Color(cardImage.color.r, cardImage.color.g, cardImage.color.b, cardImage.color.a + 0.05f);
                    anyChanged = true;
                }
                if (cardImage.rectTransform.localScale.x < 1)
                {
                    cardImage.rectTransform.localScale = new Vector3(cardImage.rectTransform.localScale.x + 0.05f, cardImage.rectTransform.localScale.y + 0.05f, 1);
                    anyChanged = true;
                }

                if (!anyChanged)
                {
                    break;
                }

                yield return new WaitForSeconds(0.01f);
            }

            yield return new WaitForSeconds(1);

            while (cardImage.color.a > 0)
            {
                cardImage.color = new Color(cardImage.color.r, cardImage.color.g, cardImage.color.b, cardImage.color.a - 0.05f);
                yield return new WaitForSeconds(0.01f);
            }

            GameEventHandler.Instance.EventCompleted();
        }
    }
}
