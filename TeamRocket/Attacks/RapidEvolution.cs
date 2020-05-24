using NetworkingCore;
using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TCGCards.Core;

namespace TeamRocket.Attacks
{
    internal class RapidEvolution : Attack, IDeckSearcher
    {
        public RapidEvolution()
        {
            Cost = new List<Energy>
            {
                new Energy(EnergyTypes.Water, 3)
            };
            Name = "Rapid Evolution";
            Description = "Search your deck for a card that evolves from Magikarp and put it on this Pokémon. (This counts as evolving this Pokémon.) Shuffle your deck afterward.";
            DamageText = "";
        }

        public override Damage GetDamage(Player owner, Player opponent)
        {
            return 0;
        }

        public List<IDeckFilter> GetDeckFilters()
        {
            return new List<IDeckFilter> { new RapidEvolutionDeckFilter() };
        }

        public int GetNumberOfCards()
        {
            return 1;
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            List<NetworkId> selectedCards = this.TriggerDeckSearch(owner);

            PokemonCard evolution = owner.ActivePokemonCard.Evolve((PokemonCard)game.FindCardById(selectedCards.First()));
            owner.ActivePokemonCard = evolution;
        }
    }

    internal class RapidEvolutionDeckFilter : IDeckFilter
    {
        public bool IsCardValid(Card card)
        {
            return card is PokemonCard && ((PokemonCard)card).EvolvesFrom == PokemonNames.Magikarp;
        }
    }
}
