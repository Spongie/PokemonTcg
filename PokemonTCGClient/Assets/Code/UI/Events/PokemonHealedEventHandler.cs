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
            int currentDamage = 0;
            
            if (!string.IsNullOrEmpty(target.DamageDisplay.text.Trim()))
            {
                currentDamage = int.Parse(target.DamageDisplay.text);
            }

            int afterHeal = currentDamage - pokemonHealedEvent.Healing;
            target.DamageDisplay.text = afterHeal > 0 ? afterHeal.ToString() : string.Empty;

            target.SpawnHealEffect();
            StartCoroutine(JustWait());
        }

        IEnumerator JustWait()
        {
            yield return new WaitForSeconds(1.0f);
            GameEventHandler.Instance.EventCompleted();
        }
    }
}
