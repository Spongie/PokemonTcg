using System.Collections;
using TCGCards.Core.GameEvents;
using UnityEngine;

namespace Assets.Code.UI.Events
{
    public class AbilityActivatedEventHandler : MonoBehaviour
    {
        public void Trigger(AbilityActivatedEvent abilityEvent)
        {
            var pokemon = GameController.Instance.GetCardRendererById(abilityEvent.PokemonId);
            pokemon.SpawnAbilityEffect();
            StartCoroutine(AbilityWaitRoutine());
        }

        IEnumerator AbilityWaitRoutine()
        {
            yield return new WaitForSeconds(0.4f);

            GameEventHandler.Instance.EventCompleted();
        }
    }
}
