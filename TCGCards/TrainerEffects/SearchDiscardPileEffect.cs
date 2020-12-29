using CardEditor.Views;
using Entities;
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
        private bool shuffleIntoDeck;
        private EnergyTypes energyType = EnergyTypes.All;
        private int flipCoinsForAmount = -1;

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

        [DynamicInput("Flip coins for amount")]
        public int FlipCoinsForAmount
        {
            get { return flipCoinsForAmount; }
            set
            {
                flipCoinsForAmount = value;
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

        [DynamicInput("Energy Type", InputControl.Dropdown, typeof(EnergyTypes))]
        public EnergyTypes EnergyType
        {
            get { return energyType; }
            set
            {
                energyType = value;
                FirePropertyChanged();
            }
        }


        [DynamicInput("Shuffle into deck", InputControl.Boolean)]
        public bool ShuffleIntoDeck
        {
            get { return shuffleIntoDeck; }
            set
            {
                shuffleIntoDeck = value;
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
            int amount = FlipCoinsForAmount != -1 ? game.FlipCoins(FlipCoinsForAmount) : Amount;

            if (amount == 0)
            {
                return;
            }

            List<Card> choices = CardUtil.GetCardsOfType(caster.DiscardPile, CardType, EnergyType);

            var message = new PickFromListMessage(choices, 0, amount).ToNetworkMessage(game.Id);
            var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards;
            var cards = new List<Card>();

            foreach (var id in response)
            {
                var card = game.Cards[id];
                caster.DiscardPile.Remove(card);
                
                if (shuffleIntoDeck)
                {
                    cards.Add(card);
                    caster.Deck.Cards.Push(card);
                }
                else
                {
                    caster.Hand.Add(card);
                    game.SendEventToPlayers(new DrawCardsEvent() { Player = caster.Id, Cards = new List<Card>() { card } });
                }
            }

            if (shuffleIntoDeck)
            {
                opponent.RevealCards(cards);
                caster.Deck.Shuffle();
            }
        }
    }
}
