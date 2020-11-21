using Assets.Code._2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCGCards.Core.GameEvents;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.UI.Events
{
    public class AttachedEnergyDiscardedEventHandler : MonoBehaviour
    {
        //public CardRenderer tempActive;

        public void Trigger(AttachedEnergyDiscardedEvent attachedEnergyDiscardedEvent) 
        {
            var targetPokemon = GameController.Instance.GetCardRendererById(attachedEnergyDiscardedEvent.FromPokemonId);

            foreach (var attachedEnergy in targetPokemon.GetComponentsInChildren<AttachedEnergy>())
            {
                if (attachedEnergy.energyCard.Id.Equals(attachedEnergyDiscardedEvent.DiscardedCard.Id))
                {
                    var rectTransform = attachedEnergy.GetComponent<RectTransform>();
                    rectTransform.SetParent(transform);
                    rectTransform.LeanScale(new Vector3(2.5f, 2.5f), 0.5f);
                    rectTransform.LeanMoveLocal(new Vector3(350, 150), 1f).setOnComplete(() =>
                    {
                        rectTransform.LeanAlpha(0f, 0.5f).setDelay(0.5f).setDestroyOnComplete(true).setOnComplete(() =>
                        {
                            GameEventHandler.Instance.EventCompleted();
                        });
                    });
                }
            }
        }
    }
}
