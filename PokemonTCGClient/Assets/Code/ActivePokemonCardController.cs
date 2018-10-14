using TCGCards;
using UnityEngine.EventSystems;

namespace Assets.Code
{
    public class ActivePokemonCardController : CardController
    {
        public AttachedCardsController attachedCardsController;
        public bool IsMouseOver;

        protected override bool ActivatePointerEnterOrExit() => false;

        public override void SetCard(Card card)
        {
            base.SetCard(card);
            attachedCardsController.SetCard(card);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            IsMouseOver = true;
            base.OnPointerEnter(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            IsMouseOver = false;
            base.OnPointerExit(eventData);
        }
    }
}
