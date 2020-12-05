using System.Collections;
using TCGCards.Core.GameEvents;
using UnityEngine;

namespace Assets.Code.UI.Events
{
    public class PokemonHealedEventHandler : MonoBehaviour
    {
        public void Trigger(PokemonHealedEvent pokemonHealedEvent)
        {
            var target = GameController.Instance.GetCardRendererById(pokemonHealedEvent.PokemonId);
            target.SpawnHealEffect();
            StartCoroutine(JustWait());
        }

        IEnumerator JustWait()
        {
            yield return new WaitForSeconds(1.0f);
        }
    }
}
