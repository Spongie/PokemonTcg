using CardEditor.Views;
using Entities;
using Entities.Models;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.GameEvents;
using TCGCards.Core.Messages;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class PickCardFromTopOfDeck : DataModel, IEffect
    {
        public string EffectType
        {
            get
            {
                return "Pick cards from top of deck";
            }
        }

        private int amountToPick;
        private int amountToPickFrom;
        private CardType cardType;
        private EnergyTypes energyType;
        private bool revealPickedCards;

        [DynamicInput("Cards to pick")]
        public int AmountToPick
        {
            get { return amountToPick; }
            set
            {
                amountToPick = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Cards to pick from")]
        public int AmountToPickFrom
        {
            get { return amountToPickFrom; }
            set
            {
                amountToPickFrom = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Card Types", InputControl.Dropdown, typeof(CardType))]
        public CardType CardType
        {
            get { return cardType; }
            set
            {
                cardType = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("EnergyType", InputControl.Dropdown, typeof(EnergyTypes))]
        public EnergyTypes EnergyType
        {
            get { return energyType; }
            set
            {
                energyType = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Reveal picked cards", InputControl.Boolean)]
        public bool RevealPickedCards
        {
            get { return revealPickedCards; }
            set
            {
                revealPickedCards = value;
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
            var choices = caster.Deck.Cards.Take(AmountToPickFrom).ToList();

            var message = new DeckSearchMessage(choices, CardUtil.GetCardFilters(CardType, EnergyType), AmountToPick);
            var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message.ToNetworkMessage(game.Id));
            var cards = new HashSet<Card>(response.Cards.Select(id => game.Cards[id]));

            var discardedCards = new List<Card>();
            var cardsDrawn = new List<Card>();

            foreach (var card in choices)
            {
                if (cards.Contains(card))
                {
                    if (revealPickedCards)
                    {
                        card.RevealToAll();
                    }
                    cardsDrawn.Add(card);
                }
                else
                {
                    card.RevealToAll();
                    discardedCards.Add(card);
                }
            }

            caster.DrawCardsFromDeck(cardsDrawn);
            caster.DiscardPile.AddRange(discardedCards);

            game.SendEventToPlayers(new CardsDiscardedEvent() { Cards = discardedCards, Player = caster.Id });
        }
    }
}
