using NetworkingCore;
using UnityEngine;

namespace Assets.Code.UI.Events
{
    public class PokemonAttackedEventHandler : MonoBehaviour
    {
        public void TriggerEvent(NetworkId attackingPlayer)
        {
            var attackingPokemon = GameController.Instance.Player.Id.Equals(attackingPlayer) ? 
                GameController.Instance.Player.ActivePokemonCard 
                : GameController.Instance.OpponentPlayer.ActivePokemonCard;

            var defending = GameController.Instance.Player.Id.Equals(attackingPlayer) ?
                GameController.Instance.OpponentPlayer.ActivePokemonCard
                : GameController.Instance.Player.ActivePokemonCard;

            var cardRenderer = GameController.Instance.GetCardRendererById(attackingPokemon.Id);
            var startPos = cardRenderer.GetComponent<RectTransform>().position;
            var targetPos = GameController.Instance.GetCardRendererById(defending.Id).GetComponent<RectTransform>().position;
            var canvas = cardRenderer.GetComponent<Canvas>();
            canvas.sortingOrder += 1;

            cardRenderer.gameObject.LeanMove(targetPos, 0.5f).setEaseInCubic().setOnComplete(() =>
            {
                cardRenderer.gameObject.LeanMove(startPos, 0.4f).setEaseInOutCubic().setOnComplete(() =>
                {
                    canvas.sortingOrder -= 1;
                });

                GameEventHandler.Instance.EventCompleted();
            });
        }

    }
}
