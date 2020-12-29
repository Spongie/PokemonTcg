using CardEditor.Views;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class PutCardFromDiscardOnDeck : DataModel, IEffect
    {
        private CardType cardType;
        private int amount = 1;
        private bool coinFlip;
        private bool targetsOpponent;

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

        [DynamicInput("Opponent", InputControl.Boolean)]
        public bool TargetsOpponent
        {
            get { return targetsOpponent; }
            set
            {
                targetsOpponent = value;
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
                case CardType.BasicPokemon:
                    return caster.DiscardPile.OfType<PokemonCard>().Where(pokemon => pokemon.Stage == 0).Count() >= Amount;
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

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            if (CoinFlip && game.FlipCoins(1) == 0)
            {
                return;
            }

            var target = TargetsOpponent ? opponent : caster;

            List<Card> choices = CardUtil.GetCardsOfType(target.DiscardPile, CardType);

            var message = new PickFromListMessage(choices, Amount).ToNetworkMessage(game.Id);
            var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards;

            foreach (var id in response)
            {
                var card = game.Cards[id];
                target.Deck.Cards.Push(card);
                target.DiscardPile.Remove(card);
            }
        }
    }
}
