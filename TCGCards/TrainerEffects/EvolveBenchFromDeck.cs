using System.Collections.Generic;
using System.Linq;
using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;
using TCGCards.Core.Deckfilters;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class EvolveBenchFromDeck : DataModel, IEffect
    {
        private CoinFlipConditional coinFlipConditional = new CoinFlipConditional();

        [DynamicInput("Condition", InputControl.Dynamic)]
        public CoinFlipConditional CoinFlipConditional
        {
            get { return coinFlipConditional; }
            set
            {
                coinFlipConditional = value;
                FirePropertyChanged();
            }
        }

        public string EffectType
        {
            get
            {
                return "Search deck for evolution and evolve";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return true;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            if (!CoinFlipConditional.IsOk(game, caster))
            {
                return;
            }

            var target = Targeting.AskForTargetFromTargetingMode(TargetingMode.YourBench, game, caster, opponent, pokemonSource);

            if (target == null)
            {
                return;
            }

            var filter = new EvolvesFromPokemonFilter() { Name = target.Name };

            var response = DeckSearchUtil.SearchDeck(game, caster, new List<IDeckFilter> { filter }, 1);

            if (response.Count == 0)
            {
                return;
            }

            game.EvolvePokemon(target, (PokemonCard)response[0], true);
            caster.Deck.Cards = new Stack<Card>(caster.Deck.Cards.Except(response).ToList());
            caster.Deck.Shuffle();
        }
    }
}
