using CardEditor.Views;
using Entities.Models;
using System.Collections.Generic;
using TCGCards.Core;
using TCGCards.Core.Deckfilters;

namespace TCGCards.TrainerEffects
{
    public class SearchDeckForEvolutionCard : DataModel, IEffect
    {
        public string EffectType
        {
            get
            {
                return "Search deck for evolution";
            }
        }

        private int amount;
        private bool revealCard;

        [DynamicInput("Reveal card", InputControl.Boolean)]
        public bool RevealCard
        {
            get { return revealCard; }
            set
            {
                revealCard = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Number of cards")]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                FirePropertyChanged();
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
            foreach (var card in DeckSearchUtil.SearchDeck(game, caster, new List<IDeckFilter> { new EvolutionPokemonFilter() }, Amount))
            {
                if (revealCard)
                {
                    card.RevealToAll();
                }

                caster.DrawCardsFromDeck(new List<Card> { card });
            }

            caster.Deck.Shuffle();
        }
    }
}
