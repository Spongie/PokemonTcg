using CardEditor.Views;
using Entities.Models;
using System.Collections.Generic;
using TCGCards.Core;
using TCGCards.Core.GameEvents;
using TCGCards.Core.Messages;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class LookAtAndDiscardCardFromOpponent : DataModel, IEffect
    {
        public string EffectType
        {
            get
            {
                return "Look and discard card from opponent";
            }
        }

        private CardType cardType;
        private int amount;
        private bool shuffleIntoDeck;

        [DynamicInput("Cards to discard")]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Cardtype to discard", InputControl.Dropdown, typeof(CardType))]
        public CardType CardType
        {
            get { return cardType; }
            set
            {
                cardType = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Shuffle into deck instead", InputControl.Boolean)]
        public bool ShuffleIntoDeck
        {
            get { return shuffleIntoDeck; }
            set
            {
                shuffleIntoDeck = value;
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
            var filters = CardUtil.GetCardFilters(CardType);
            var searchMessage = new DeckSearchMessage(opponent.Hand, filters, Amount);
            var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(searchMessage.ToNetworkMessage(game.Id));

            foreach (var id in response.Cards)
            {
                var card = game.Cards[id];
                
                if (shuffleIntoDeck)
                {
                    opponent.Hand.Remove(card);
                    opponent.Deck.ShuffleInCard(card);

                    game.SendEventToPlayers(new CardsDiscardedEvent() { Cards = new List<Card> { card }, Player = opponent.Id });
                }
                else
                {
                    opponent.DiscardCard(card);
                }
            }
        }
    }
}
