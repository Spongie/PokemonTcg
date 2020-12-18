using TCGCards;
using TCGCards.Core.GameEvents;
using UnityEngine;

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
            renderer.GetComponent<RectTransform>().LeanAlpha(0, 1f).setOnComplete(() =>
            {
                if (isMyPokemon)
                {
                    GameController.Instance.Player.BenchedPokemon.Remove(pokemon);

                    if (bounceEvent.ToHand)
                    {
                        GameController.Instance.playerHand.AddCardToHand(pokemon);
                    }
                }
                else
                {
                    GameController.Instance.OpponentPlayer.BenchedPokemon.Remove(pokemon);
                }

                Destroy(renderer.gameObject);
                GameEventHandler.Instance.EventCompleted();
            });
        }
    }
}
