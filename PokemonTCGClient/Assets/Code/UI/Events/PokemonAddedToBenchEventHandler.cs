using Assets.Code._2D;
using TCGCards.Core.GameEvents;
using UnityEngine;

namespace Assets.Code.UI.Events
{
    public class PokemonAddedToBenchEventHandler : MonoBehaviour
    {
        public GameObject CardPrefab;
        public BenchController OpponentBench;
        public BenchController PlayerBench;

        public void Trigger(PokemonAddedToBenchEvent addedToBenchEvent)
        {
            GameController.Instance.playerHand.RemoveCard(addedToBenchEvent.Pokemon);
            bool isMyPokemon = addedToBenchEvent.Player.Equals(GameController.Instance.myId);
            BenchController targetBench = isMyPokemon ? PlayerBench : OpponentBench;
            var player = isMyPokemon ? GameController.Instance.Player : GameController.Instance.OpponentPlayer;

            var targetParent = isMyPokemon ? PlayerBench.GetSlot(addedToBenchEvent.Index) : OpponentBench.GetSlot(addedToBenchEvent.Index);

            var spawnedObject = Instantiate(CardPrefab, transform);
            var rectTransform = spawnedObject.GetComponent<RectTransform>();
            var renderer = spawnedObject.GetComponent<CardRenderer>();
            renderer.SetCard(addedToBenchEvent.Pokemon, true);

            rectTransform.localPosition = Vector3.zero;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 220);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 270);
            
            
            GameController.Instance.AddCard(renderer);
            GameController.Instance.Player?.Hand.Remove(addedToBenchEvent.Pokemon);

            var canvas = renderer.GetComponent<Canvas>();
            var oldSortorder = canvas.sortingOrder;
            canvas.sortingOrder = 9999;

            spawnedObject.tag = "Ignore";
            rectTransform.transform.SetParent(targetParent.transform, true);

            rectTransform.LeanMove(Vector3.zero, 0.5f).setDelay(0.5f);
            rectTransform.LeanScale(new Vector3(1, 1, 1), 0.25f).setDelay(0.55f).setOnComplete(() =>
            {
                spawnedObject.tag = "Untagged";
                canvas.sortingOrder = 10;
                GameEventHandler.Instance.EventCompleted();
            });
        }
    }
}
