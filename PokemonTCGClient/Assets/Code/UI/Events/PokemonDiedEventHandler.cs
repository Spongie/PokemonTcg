using TCGCards.Core.GameEvents;
using UnityEngine;

namespace Assets.Code.UI.Events
{
    public class PokemonDiedEventHandler : MonoBehaviour
    {
        public void Trigger(PokemonDiedEvent deathEvent)
        {
            var targetRenderer = GameController.Instance.GetCardRendererById(deathEvent.Pokemon.Id);

            targetRenderer.SpawnDeathEffect();
            targetRenderer.GetComponent<RectTransform>().LeanAlpha(0, 1.0f).setDelay(0.25f).setDestroyOnComplete(true).setOnComplete(() =>
            {
                Destroy(targetRenderer.gameObject);
                GameEventHandler.Instance.EventCompleted();
            });
        }
    }
}
