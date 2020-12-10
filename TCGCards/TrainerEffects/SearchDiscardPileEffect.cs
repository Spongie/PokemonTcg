using CardEditor.Views;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.GameEvents;
using TCGCards.Core.Messages;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class SearchDiscardPileEffect : DataModel, IEffect
    {
        private CardType cardType;
        private int amount = 1;

        [DynamicInput("Cards to pick up")]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                FirePropertyChanged();
            }
        }

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
                return "Pick from discard";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return CardUtil.GetCardsOfType(caster.DiscardPile, CardType).Any();
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            throw new NotImplementedException();
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            List<Card> choices = CardUtil.GetCardsOfType(caster.DiscardPile, CardType);

            var message = new PickFromListMessage(choices, 0, Amount).ToNetworkMessage(game.Id);
            var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards;

            foreach (var id in response)
            {
                var card = game.FindCardById(id);
                caster.Hand.Add(card);
                caster.DiscardPile.Remove(card);

                game.SendEventToPlayers(new DrawCardsEvent() { Amount = 1, Player = caster.Id, Cards = new List<Card>() { card } });
            }
        }
    }
}
