using Assets.Code._2D;
using TCGCards;
using TCGCards.Core.GameEvents;
using UnityEngine;

namespace Assets.Code.UI.Events
{
    public class PokemonRemovedFromBenchEventHandler : MonoBehaviour
    {
        public BenchController playerBench;
        public BenchController opponentBench;

        public void Trigger(PokemonRemovedFromBench removedFromBenchEvent)
        {
            var pokemon = GameController.Instance.GetCardRendererById(removedFromBenchEvent.PokemonId);
            var isMyPokemon = pokemon.card.Owner.Id.Equals(GameController.Instance.myId);

            pokemon.GetComponent<RectTransform>().LeanAlpha(0, 0.75f).setOnComplete(() =>
            {
                if (isMyPokemon)
                {
                    GameController.Instance.Player.BenchedPokemon.Remove((PokemonCard)pokemon.card);
                }
                else
                {
                    GameController.Instance.OpponentPlayer.BenchedPokemon.Remove((PokemonCard)pokemon.card);
                }
                
                Destroy(pokemon.gameObject);
                GameEventHandler.Instance.EventCompleted();
            });
        }
    }
}
