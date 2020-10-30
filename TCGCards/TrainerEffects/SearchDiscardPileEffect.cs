using CardEditor.Views;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;

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
                default:
                    throw new NotImplementedException();
            }
        }

        public void Process(GameField game, Player caster, Player opponent)
        {
            IEnumerable<Card> choices;

            switch (CardType)
            {
                case CardType.Pokemon:
                    choices = caster.DiscardPile.OfType<PokemonCard>();
                    break;
                case CardType.Trainer:
                    choices = caster.DiscardPile.OfType<TrainerCard>();
                    break;
                case CardType.Energy:
                    choices = caster.DiscardPile.OfType<EnergyCard>();
                    break;
                case CardType.Any:
                default:
                    choices = caster.DiscardPile;
                    break;
            }

            var message = new PickFromListMessage(choices, Amount).ToNetworkMessage(game.Id);
            var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards;

            foreach (var id in response)
            {
                var card = game.FindCardById(id);
                caster.Hand.Add(card);
                caster.DiscardPile.Remove(card);
            }
        }
    }
}
