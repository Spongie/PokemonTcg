using System.Collections;
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
            StartCoroutine(DeathRoutine(targetRenderer));
        }

        IEnumerator DeathRoutine(CardRenderer target)
        {
            yield return new WaitForSeconds(0.25f);

            while (target.art.color.a > 0)
            {
                target.art.color = new Color(target.art.color.r, target.art.color.g, target.art.color.b, target.art.color.a - 0.05f);
                yield return new WaitForSeconds(0.025f);
            }

            yield return new WaitForSeconds(1f);

            GameEventHandler.Instance.EventCompleted();
            Destroy(target.gameObject);
        }
    }
}
