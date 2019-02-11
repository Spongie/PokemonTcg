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

                NetworkManager.Instance.gameService.EvolvePokemon((PokemonCard)cardRenderer.card, (PokemonCard)existingCard.card);
            }
        }
        if (cardRenderer.card is EnergyCard)
        {
            var existingCard = dropTarget.GetComponentInChildren<CardRenderer>();

            if (existingCard == null)
            {
                return;
            }

            NetworkManager.Instance.gameService.AttachEnergy((PokemonCard)existingCard.card, (EnergyCard)cardRenderer.card);
        }
    }
}
