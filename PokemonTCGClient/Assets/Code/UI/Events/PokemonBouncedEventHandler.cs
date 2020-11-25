using TCGCards.Core.GameEvents;
using UnityEngine;

namespace Assets.Code.UI.Events
{
    public class PokemonBouncedEventHandler : MonoBehaviour
    {
        public void Trigger(PokemonBouncedEvent bounceEvent)
        {
            var renderer = GameController.Instance.GetCardRendererById(bounceEvent.PokemonId);

            renderer.SpawnBounceEffect();
            renderer.GetComponent<RectTransform>().LeanAlpha(0, 1f).setOnComplete(() =>
            {
                GameEventHandler.Instance.EventCompleted();
            });
        }
    }
}
