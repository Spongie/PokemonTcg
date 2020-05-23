using Assets.Code;
using TCGCards;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActiveDropZone : MonoBehaviour, IDropHandler
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

            if (card.Stage > 0)
            {
                var existingCard = dropTarget.GetComponentInChildren<CardRenderer>();

                if (existingCard == null)
                {
                    return;
                }

                NetworkManager.Instance.gameService.EvolvePokemon(GameController.Instance.gameField.Id, cardRenderer.card.Id, existingCard.card.Id);
            }
        }
        if (cardRenderer.card is EnergyCard)
        {
            var existingCard = dropTarget.GetComponentInChildren<CardRenderer>();

            if (existingCard == null)
            {
                return;
            }

            NetworkManager.Instance.gameService.AttachEnergy(GameController.Instance.gameField.Id, existingCard.card.Id, cardRenderer.card.Id);
        }
    }
}
