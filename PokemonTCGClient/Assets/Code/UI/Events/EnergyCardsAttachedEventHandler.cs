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

            cardImage.sprite = null;
            cardImage.rectTransform.localScale = new Vector3(0.1f, 0.1f, 1);
            cardImage.color = new Color(cardImage.color.r, cardImage.color.g, cardImage.color.b, 0);

            loader.LoadSprite(card, cardImage);

            cardImage.rectTransform.LeanAlpha(1.0f, 0.2f);
            cardImage.rectTransform.LeanScale(new Vector3(1.0f, 1.0f, 1.0f), 0.25f).setOnComplete(() =>
            {
                cardImage.rectTransform.LeanScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f).setDelay(0.25f).setOnComplete(() =>
                {
                    var target = GameController.Instance.GetCardRendererById(attachedTo.Id);
                    var targetTransform = target.GetComponent<RectTransform>();
                    cardImage.sprite = energyResourceManager.GetSpriteForEnergyCard(card);
                    cardImage.color = new Color(cardImage.color.r, cardImage.color.g, cardImage.color.b, 0);
                    var attachedEnergy = Instantiate(attachedEnergyPrefab, attachedParent.transform);

                    var energyRectTransform = attachedEnergy.GetComponent<RectTransform>();
                    energyRectTransform.localPosition = new Vector3(400, 0);
                    attachedEnergy.transform.SetParent(targetTransform.transform);
                    attachedEnergy.GetComponent<Image>().sprite = EnergyResourceManager.Instance.GetSpriteForEnergyCard(card);

                    energyRectTransform.LeanMove(new Vector3(20 * (attachedTo.AttachedEnergy.Count + 1), -targetTransform.rect.height + 20, 0), 0.75f).setEaseOutCirc().setDestroyOnComplete(true).setOnComplete(() =>
                    {
                        target.insertAttachedEnergy(card);
                        GameEventHandler.Instance.EventCompleted();
                    });
                });
            });
        }
    }
}
