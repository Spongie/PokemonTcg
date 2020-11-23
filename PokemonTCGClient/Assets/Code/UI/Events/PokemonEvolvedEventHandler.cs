using System.Collections;
using TCGCards.Core.GameEvents;
using UnityEngine;

namespace Assets.Code.UI.Events
{
    public class PokemonEvolvedEventHandler : MonoBehaviour
    {
        public void Trigger(PokemonEvolvedEvent pokemonEvolvedEvent)
        {
            GameController.Instance.playerHand.RemoveCard(pokemonEvolvedEvent.NewPokemonCard);
            var targetPokemon = GameController.Instance.GetCardRendererById(pokemonEvolvedEvent.TargetPokemonId);
            targetPokemon.SetCard(pokemonEvolvedEvent.NewPokemonCard, ZoomMode.Center, false);
            targetPokemon.SpawnEvolveEffect();
            StartCoroutine(JustWaitForComplete());
        }

        IEnumerator JustWaitForComplete()
        {
            yield return new WaitForSeconds(1f);
            GameEventHandler.Instance.EventCompleted();
        }
    } 
}
