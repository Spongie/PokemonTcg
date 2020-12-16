using CardEditor.Views;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class DiscardCardsAtRandom : DataModel, IEffect
    {
        public string EffectType
        {
            get
            {
                return "Discard at random";
            }
        }

        private bool targetsOpponent;
        private bool coinFlip;
        private int amount;
        private bool shuffleIntoDeck;

        [DynamicInput("Coin Flip", InputControl.Boolean)]
        public bool CoinFlip
        {
            get { return coinFlip; }
            set
            {
                coinFlip = value;
                FirePropertyChanged();
            }
        }

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

        [DynamicInput("Cards to discard (-1 for all)")]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
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
            var target = targetsOpponent ? opponent : caster;

            List<Card> cardsToDiscard;
            if (Amount == -1 || target.Hand.Count <= Amount)
            {
                cardsToDiscard = new List<Card>(caster.Hand);
            }
            else
            {
                var random = new Random();
                cardsToDiscard = target.Hand.OrderBy(x => random.Next(int.MaxValue)).Take(Amount).ToList();
            }

            if (ShuffleIntoDeck)
            {
                caster.Deck.ShuffleInCards(new List<Card>(caster.Hand));
                caster.Hand.Clear();
                caster.TriggerDiscardEvent(cardsToDiscard);
            }
            else
            {
                caster.DiscardCards(cardsToDiscard);
            }
        }
    }
}
