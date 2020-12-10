using CardEditor.Views;
using Entities;
using Entities.Models;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class SearchDeckForCardEffect : DataModel, IEffect
    {
        private CardType cardType;
        private EnergyTypes energyType = EnergyTypes.None;
        private bool revealCard;

        [DynamicInput("Card type", InputControl.Dropdown, typeof(CardType))]
        public CardType CardType
        {
            get { return cardType; }
            set
            {
                cardType = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Energy type (when basic energy)", InputControl.Dropdown, typeof(EnergyTypes))]
        public EnergyTypes EnergyType
        {
            get { return energyType; }
            set
            {
                energyType = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Reaveal searched card?", InputControl.Boolean)]
        public bool RevealCard
        {
            get { return revealCard; }
            set
            {
                revealCard = value;
                FirePropertyChanged();
            }
        }

        public string EffectType
        {
            get
            {
                return "Search your deck for a card";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return caster.Deck.Cards.Count > 0;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            throw new System.NotImplementedException();
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            var filter = CardUtil.GetCardFilters(CardType, EnergyType).ToList();

            foreach (var card in DeckSearchUtil.SearchDeck(game, caster, filter, 1))
            {
                if (revealCard)
                {
                    card.IsRevealed = true;
                }

                caster.DrawCardsFromDeck(new List<Card> { card });
            }

            caster.Deck.Shuffle();
        }
    }
}
