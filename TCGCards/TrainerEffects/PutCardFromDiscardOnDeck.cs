﻿using CardEditor.Views;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards.TrainerEffects
{
    public class PutCardFromDiscardOnDeck : DataModel, IEffect
    {
        private CardType cardType;
        private int amount = 1;
        private bool coinFlip;

        [DynamicInput("Flip a coin", InputControl.Boolean)]
        public bool CoinFlip
        {
            get { return coinFlip; }
            set
            {
                coinFlip = value;
                FirePropertyChanged();
            }
        }

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
                return "Put card from discard onto deck";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            switch (cardType)
            {
                case CardType.Any:
                    return caster.DiscardPile.Count() >= Amount;
                case CardType.Pokemon:
                    return caster.DiscardPile.Count(card => card is PokemonCard) >= Amount;
                case CardType.Trainer:
                    return caster.DiscardPile.Count(card => card is TrainerCard) >= Amount;
                case CardType.Energy:
                    return caster.DiscardPile.Count(card => card is EnergyCard) >= Amount;
                case CardType.BasicEnergy:
                    return caster.DiscardPile.OfType<EnergyCard>().Count(x => x.IsBasic) >= Amount;
                default:
                    throw new NotImplementedException();
            }
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent)
        {
            if (CoinFlip && game.FlipCoins(1) == 0)
            {
                return;
            }

            IEnumerable<Card> choices = CardUtil.GetCardsOfType(caster.DiscardPile, CardType);

            var message = new PickFromListMessage(choices, Amount).ToNetworkMessage(game.Id);
            var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards;

            foreach (var id in response)
            {
                var card = game.FindCardById(id);
                caster.Deck.Cards.Push(card);
                caster.DiscardPile.Remove(card);
            }
        }
    }
}