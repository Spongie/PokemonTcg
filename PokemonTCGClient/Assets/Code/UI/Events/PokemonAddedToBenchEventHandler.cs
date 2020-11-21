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
            bool isMyPokemon = addedToBenchEvent.Player.Equals(GameController.Instance.myId);
            GameObject targetParent = isMyPokemon ? PlayerBench : OpponentBench;
            ZoomMode zoomMode = isMyPokemon ? ZoomMode.FromBottom : ZoomMode.FromTop;

            var spawnedObject = Instantiate(CardPrefab, transform);
            var rectTransform = spawnedObject.GetComponent<RectTransform>();
            spawnedObject.GetComponent<CardRenderer>().SetCard(addedToBenchEvent.Pokemon, zoomMode, true);

            rectTransform.localScale = new Vector3(2, 2, 1);
            rectTransform.localPosition = new Vector3(-50, 270, 0);
            spawnedObject.tag = "Ignore";
            rectTransform.transform.SetParent(targetParent.transform);

            rectTransform.LeanMoveLocal(new Vector3(900, 0, 0), 0.5f).setDelay(0.5f);
            rectTransform.LeanScale(new Vector3(1, 1, 1), 0.25f).setDelay(0.5f).setOnComplete(() =>
            {
                spawnedObject.tag = "Untagged";
                GameEventHandler.Instance.EventCompleted();
            });
        }
    }
}
