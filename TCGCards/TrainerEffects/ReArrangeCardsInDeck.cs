using CardEditor.Views;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards.TrainerEffects
{
    public class ReArrangeCardsInDeck : DataModel, IEffect
    {
        private int amount;
        private bool targetsOpponent;

        [DynamicInput("Targets opponent", InputControl.Boolean)]
        public bool TargetsOpponent
        {
            get { return targetsOpponent; }
            set
            {
                targetsOpponent = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Amount")]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                FirePropertyChanged();
            }
        }

        public string EffectType
        {
            get
            {
                return "ReArrange cards on the top of deck";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return true;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent)
        {
            var target = TargetsOpponent ? opponent : caster;

            var cards = new List<Card>();

            for (int i = 0; i < Amount; i++)
            {
                if (target.Deck.Cards.Count > 0)
                {
                    cards.Add(target.Deck.Cards.Pop());
                }
            }
            
            if (cards.Count == 0)
            {
                return;
            }

            var message = new PickFromListMessage(cards, 1).ToNetworkMessage(game.Id);
            var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message);

            foreach (var id in response.Cards)
            {
                var card = cards.First(x => x.Id.Equals(id));
                target.Deck.Cards.Push(card);
            }
        }
    }
}
