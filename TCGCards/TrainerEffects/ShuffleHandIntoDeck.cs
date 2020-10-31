using CardEditor.Views;
using Entities.Models;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards.TrainerEffects
{
    public class ShuffleHandIntoDeck : DataModel, IEffect
    {
        private bool opponents;
        private int amount = -1;

        [DynamicInput("Targets opponent?", InputControl.Boolean)]
        public bool Opponents
        {
            get { return opponents; }
            set
            {
                opponents = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Cards to shuffle in (-1 for all)")]
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
                return "Shuffle hand into deck";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent) => true;

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand)
        {
            throw new System.NotImplementedException();
        }

        public void Process(GameField game, Player caster, Player opponent)
        {
            var target = opponents ? opponent : caster;

            if (amount == -1)
            {
                target.Deck.ShuffleInCards(opponent.Hand);
                target.Hand.Clear();
            }
            else
            {
                var message = new DiscardCardsMessage(Amount);
                var choices = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message.ToNetworkMessage(game.Id)).Cards;

                var cards = choices.Select(id => game.FindCardById(id)).ToList();

                foreach (var card in cards)
                {
                    target.Hand.Remove(card);
                }

                target.Deck.ShuffleInCards(cards);
            }
        }
    }
}
