using System.Collections.Generic;
using CardEditor.Views;
using Entities.Models;
using NetworkingCore;
using TCGCards.Core;
using TCGCards.Core.Deckfilters;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class SearchDeckExactCard : DataModel, IEffect
    {
        private string name;
        private CoinFlipConditional coinFlipConditional = new CoinFlipConditional();
        private bool inverted;

        public string EffectType
        {
            get
            {
                return "Search deck specific";
            }
        }

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

        [DynamicInput("Card name")]
        public string CardName
        {
            get { return name; }
            set
            {
                name = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Inverted", InputControl.Boolean)]
        public bool Inverted
        {
            get { return inverted; }
            set
            {
                inverted = value;
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
            if (!CoinFlipConditional.IsOk(game, caster))
            {
                return;
            }

            var filter = new ExactCardFilter() { Name = CardName, Invert = Inverted };

            foreach (var card in DeckSearchUtil.SearchDeck(game, caster, new List<IDeckFilter> { filter }, 1))
            {
                card.RevealToAll();
                caster.DrawCardsFromDeck(new List<NetworkId> { card.Id });
            }
        }
    }
}
