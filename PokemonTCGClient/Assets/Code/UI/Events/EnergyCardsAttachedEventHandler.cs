using Assets.Code._2D;
using System.Collections;
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
            GameController.Instance.playerHand.RemoveCard(card);
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
            cardImage.sprite = energyResourceManager.GetSpriteForEnergyCard(card);
            cardImage.color = new Color(cardImage.color.r, cardImage.color.g, cardImage.color.b, 0);
            var attachedEnergy = Instantiate(attachedEnergyPrefab, attachedParent.transform);
            
            attachedEnergy.GetComponent<RectTransform>().localPosition = new Vector3(400, 0);
            attachedEnergy.transform.SetParent(targetTransform.transform);
            attachedEnergy.GetComponent<Image>().sprite = EnergyResourceManager.Instance.GetSpriteForEnergyCard(card);

            attachedEnergy.LeanMoveLocal(new Vector3(20, -targetTransform.rect.height + 30, 0), 0.75f).setEaseOutCirc().setDestroyOnComplete(true).setOnComplete(() =>
            {
                target.insertAttachedEnergy(card);
                GameEventHandler.Instance.EventCompleted();
            });
        }
    }
}
