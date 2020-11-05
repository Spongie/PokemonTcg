using Entities;
using TCGCards;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Code._2D
{
    public class SelectEnergySymbolHandler : MonoBehaviour, IPointerClickHandler
    {
        public EnergyTypes EnergyType;

        public void OnPointerClick(PointerEventData eventData)
        {
            GameController.Instance.OnEnergyTypeClicked(EnergyType);
        }
    }
}
