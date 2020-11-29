using TCGCards.Core.GameEvents;
using UnityEngine;

namespace Assets.Code.UI.Events
{
    public class PokemonRemovedFromBenchEventHandler : MonoBehaviour
    {
        public void Trigger(PokemonRemovedFromBench removedFromBenchEvent)
        {
            var pokemon = GameController.Instance.GetCardRendererById(removedFromBenchEvent.PokemonId);

            pokemon.GetComponent<RectTransform>().LeanAlpha(0, 0.75f).setOnComplete(() =>
            {
                GameController.Instance.playerHand.RemoveCardById(removedFromBenchEvent.PokemonId);
                GameEventHandler.Instance.EventCompleted();
            });
        }
    }
}
