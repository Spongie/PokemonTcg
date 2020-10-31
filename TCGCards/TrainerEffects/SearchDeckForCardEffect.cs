using CardEditor.Views;
using Entities.Models;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Deckfilters;
using TCGCards.Core.Messages;

namespace TCGCards.TrainerEffects
{
    public class SearchDeckForCardEffect : DataModel, IEffect
    {
        private CardType cardType;

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

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand)
        {
            throw new System.NotImplementedException();
        }

        public void Process(GameField game, Player caster, Player opponent)
        {
            var filter = CardUtil.GetCardFilters(CardType).ToList();

            var message = new DeckSearchMessage(caster.Deck, filter, 1).ToNetworkMessage(game.Id);
            var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.First();

            var card = game.FindCardById(response);
            caster.DrawCardsFromDeck(new[] { card });

            caster.Deck.Shuffle();
        }
    }
}
