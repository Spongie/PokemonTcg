using System.Collections;
using TCGCards;
using TCGCards.Core.GameEvents;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.UI.Events
{
    public class PokemonBouncedEventHandler : MonoBehaviour
    {
        public void Trigger(PokemonBouncedEvent bounceEvent)
        {
            var renderer = GameController.Instance.GetCardRendererById(bounceEvent.PokemonId);
            var isMyPokemon = renderer.card.Owner.Id.Equals(GameController.Instance.myId);
            var pokemon = (PokemonCard)renderer.card;

            renderer.SpawnBounceEffect();
            StartCoroutine(FadeOutRoutine(pokemon, renderer.gameObject));
        }

        IEnumerator FadeOutRoutine(PokemonCard pokemon, GameObject gameObject)
        {
            var image = gameObject.GetComponent<Image>();
            var isMyPokemon = pokemon.Owner.Id.Equals(GameController.Instance.myId);

            while (image.color.a > 0)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - 0.05f);
                yield return new WaitForSeconds(0.025f);
            }

            if (isMyPokemon)
            {
                GameController.Instance.Player.BenchedPokemon.Remove(pokemon);
            }
            else
            {
                GameController.Instance.OpponentPlayer.BenchedPokemon.Remove(pokemon);
            }

            Destroy(gameObject);
            GameEventHandler.Instance.EventCompleted();
        }
    }
}
