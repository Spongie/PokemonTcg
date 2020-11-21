using TCGCards.Core.GameEvents;
using UnityEngine;

namespace Assets.Code.UI.Events
{
    public class PokemonEvolvedEventHandler : MonoBehaviour
    {
        public void Trigger(PokemonEvolvedEvent pokemonEvolvedEvent)
        {
            var targetPokemon = GameController.Instance.GetCardRendererById(pokemonEvolvedEvent.TargetPokemonId);
            targetPokemon.SetCard(pokemonEvolvedEvent.NewPokemonCard, ZoomMode.Center, false);
            targetPokemon.SpawnEvolveEffect();
            GameEventHandler.Instance.EventCompleted();
        }
    } 
}
