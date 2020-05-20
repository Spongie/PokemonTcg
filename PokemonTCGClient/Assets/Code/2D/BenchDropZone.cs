using Assets.Code;
using TCGCards;
using UnityEngine;
using UnityEngine.EventSystems;

public class BenchDropZone : MonoBehaviour, IDropHandler
{
    public GameObject dropTarget;

    public void OnDrop(PointerEventData eventData)
    {
        if (!GameController.Instance.IsMyTurn)
        {
            return;
        }

        var cardRenderer = eventData.pointerDrag.GetComponentInChildren<CardRenderer>();

        if (cardRenderer.card is PokemonCard)
        {
            var card = (PokemonCard)cardRenderer.card;

            if (card.Stage == 0 && dropTarget.transform.childCount == 1)
            {
                return;
            }
            else if (card.Stage == 0)
            {
                NetworkManager.Instance.gameService.SetActivePokemon(GameController.Instance.myId, card.Id);
            }
            else if (card.Stage > 0)
            {
                var existingCard = dropTarget.GetComponentInChildren<CardRenderer>();

                if (existingCard == null)
                {
                    return;
                }

                NetworkManager.Instance.gameService.EvolvePokemon(cardRenderer.card.Id, existingCard.card.Id);
            }
        }
        else if (cardRenderer.card is EnergyCard)
        {
            var existingCard = dropTarget.GetComponentInChildren<CardRenderer>();

            if (existingCard == null)
            {
                return;
            }

            NetworkManager.Instance.gameService.AttachEnergy(existingCard.card.Id, cardRenderer.card.Id);
        }

        var draggedObject = eventData.pointerDrag;
        var parent = draggedObject.transform.parent;
        parent.SetParent(dropTarget.transform);

        var parentRect = parent.GetComponent<RectTransform>();
        parentRect.localRotation = Quaternion.identity;
        parentRect.anchoredPosition = Vector3.zero;
        parentRect.localPosition = new Vector3(parent.localPosition.x, parent.localPosition.y, 0);
        parentRect.localScale = new Vector3(1, 1, 1);


        var rect = draggedObject.GetComponent<RectTransform>();
        rect.localRotation = Quaternion.identity;
        rect.localPosition = Vector3.zero;
        rect.localScale = new Vector3(1, 1, 1);

        draggedObject.GetComponent<CardDragger>().OnEndDrag(eventData);
        draggedObject.GetComponent<CardZoomer>().enabled = false;
        draggedObject.GetComponent<CardDragger>().enabled = false;
    }
}