using Assets.Code._2D;
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
    public class EnergyCardsAttachedEventHandler : MonoBehaviour
    {
        public Image cardImage;
        public CardImageLoader loader;
        public EnergyResourceManager energyResourceManager;
        public GameObject attachedEnergyPrefab;
        public GameObject attachedParent;

        public void TriggerCardPlayer(EnergyCard card, PokemonCard attachedTo)
        {
            StartCoroutine(DisplayCardEnumerator(card, attachedTo));
        }

        IEnumerator DisplayCardEnumerator(EnergyCard card, PokemonCard attachedTo)
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

            yield return new WaitForSeconds(0.25f);

            while (cardImage.rectTransform.localScale.x > 0.1f)
            {
                cardImage.rectTransform.localScale = new Vector3(cardImage.rectTransform.localScale.x - 0.05f, cardImage.rectTransform.localScale.y - 0.05f, 1);
                yield return new WaitForEndOfFrame();
            }

            var target = GameController.Instance.GetCardRendererById(attachedTo.Id);
            var targetTransform = target.GetComponent<RectTransform>();
            cardImage.sprite = null;// energyResourceManager.GetSpriteForEnergyCard(card);
            cardImage.color = new Color(cardImage.color.r, cardImage.color.g, cardImage.color.b, 0);
            var attachedEnergy = Instantiate(attachedEnergyPrefab, attachedParent.transform);
            //attachedEnergy.GetComponent<Canvas>().sortingOrder = 2786316;

            var corners = new Vector3[4];
            targetTransform.GetWorldCorners(corners);

            var x = corners.Average(corner => corner.x);
            var y = corners.Average(corner => corner.y);

            var startPos = new Vector3(x, y, 0);
            attachedEnergy.GetComponent<RectTransform>().position = startPos;
            var targetPos = new Vector3(corners[0].x + 7, corners[0].y + 25, corners[0].z);

            var points = new Vector3[]
            {
                startPos,
                new Vector3(x - 60, y + 5, 0),
                new Vector3(x - 40, y + 10, 0),
                targetPos
            };

            attachedEnergy.LeanMove(new LTBezierPath(points), 1f).setEaseOutCirc().destroyOnComplete = true;
            
            yield return new WaitForSeconds(1f);

            target.insertAttachedEnergy(card);

            GameEventHandler.Instance.EventCompleted();
        }
    }
}
