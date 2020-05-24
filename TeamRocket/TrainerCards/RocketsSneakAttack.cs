﻿using System.Linq;
using TCGCards;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TeamRocket.TrainerCards
{
    public class RocketsSneakAttack : TrainerCard
    {
        public RocketsSneakAttack()
        {
            Name = "Rocket's Sneak Attack";
            Description = "Look at your opponent's hand. If he or she has any Trainer Cards, choose 1 of them. Your opponent shuffles that card into his or her deck.";
        }

        public override void Process(GameField game, Player caster, Player opponent)
        {
            var message = new PickFromListMessage(opponent.Hand, 1);
            var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message.ToNetworkMessage(caster.Id));

            var pickedCard = game.FindCardById(response.Cards.FirstOrDefault());

            if (pickedCard != null)
            {
                opponent.Hand.Remove(pickedCard);
                opponent.Deck.Cards.Push(pickedCard);
                opponent.Deck.Shuffle();
            }
        }

        private class DeckFilter : IDeckFilter
        {
            public bool IsCardValid(Card card)
            {
                return card is TrainerCard;
            }
        }
    }
}
