using Assets.Code.UI.Gameplay;
using TCGCards.Core.GameEvents;
using UnityEngine;

namespace Assets.Code.UI.Events
{
    public class PokemonAddedToBenchEventHandler : MonoBehaviour
    {
        public GameObject CardPrefab;
        public GameObject OpponentBench;
        public GameObject PlayerBench;

        public void Trigger(PokemonAddedToBenchEvent addedToBenchEvent)
        {
            GameController.Instance.playerHand.RemoveCard(addedToBenchEvent.Pokemon);
            bool isMyPokemon = addedToBenchEvent.Player.Equals(GameController.Instance.myId);
            GameObject targetParent = isMyPokemon ? PlayerBench : OpponentBench;
            ZoomMode zoomMode = isMyPokemon ? ZoomMode.FromBottom : ZoomMode.FromTop;

            var spawnedObject = Instantiate(CardPrefab, transform);
            var rectTransform = spawnedObject.GetComponent<RectTransform>();
            var renderer = spawnedObject.GetComponent<CardRenderer>();
            renderer.SetCard(addedToBenchEvent.Pokemon, zoomMode, true);
            
            GameController.Instance.AddCard(renderer);

            var canvas = renderer.GetComponent<Canvas>();
            var oldSortorder = canvas.sortingOrder;
            canvas.sortingOrder = 9999;

            var targetPosition = targetParent.GetComponent<CardZone>().GetNextChildPosition();

            rectTransform.localScale = new Vector3(2, 2, 1);
            rectTransform.localPosition = new Vector3(-50, 270, 0);
            spawnedObject.tag = "Ignore";
            rectTransform.transform.SetParent(targetParent.transform);

            rectTransform.LeanMoveLocal(targetPosition, 0.5f).setDelay(0.5f);
            rectTransform.LeanScale(new Vector3(1, 1, 1), 0.25f).setDelay(0.5f).setOnComplete(() =>
            {
                spawnedObject.tag = "Untagged";
                canvas.sortingOrder = oldSortorder;
                GameEventHandler.Instance.EventCompleted();
            });
        }
    }
}
