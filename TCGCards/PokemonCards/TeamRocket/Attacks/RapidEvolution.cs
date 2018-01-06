using System.Collections.Generic;
using TCGCards.Core;

namespace TCGCards.PokemonCards.TeamRocket.Attacks
{
    public class RapidEvolution : Attack
    {
        public RapidEvolution()
        {
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Water, 3)
            };
            Name = "Rapid Evolution";
            Description = "Search your deck for a card that evolves from Magikarp and put it on this Pokémon. (This counts as evolving this Pokémon.) Shuffle your deck afterward.";
        }

        public override int GetDamage(Player owner, Player opponent)
        {
            return 0;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            NeedsPlayerInteraction = true;
            game.StateQueue.Enqueue(GameFieldState.EndAttack);
            game.TriggerDeckSearch(owner, new List<IDeckFilter>(), 1, PostDeckSearch);
        }

        public void PostDeckSearch(GameField game, List<ICard> pickedCards)
        {
            game.PostAttack();
        }
    }

    public class RapidEvolutionDeckFilter : IDeckFilter
    {
        public bool IsCardValid(ICard card)
        {
            return card is IPokemonCard && ((IPokemonCard)card).EvolvesFrom == PokemonNames.Magikarp;
        }
    }
}
