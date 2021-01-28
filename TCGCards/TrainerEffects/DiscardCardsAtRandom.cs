using CardEditor.Views;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class DiscardCardsAtRandom : DataModel, IEffect
    {
        private bool targetsOpponent;
        private int amount;
        private bool shuffleIntoDeck;
        private CoinFlipConditional coinflipConditional = new CoinFlipConditional();

        public string EffectType
        {
            get
            {
                return "Discard at random";
            }
        }

        [DynamicInput("Condition", InputControl.Dynamic)]
        public CoinFlipConditional CoinflipConditional
        {
            get { return coinflipConditional; }
            set
            {
                coinflipConditional = value;
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

            if (!CoinflipConditional.IsOk(game, caster))
            {
                return;
            }

            List<Card> cardsToDiscard;
            if (Amount == -1 || target.Hand.Count <= Amount)
            {
                cardsToDiscard = new List<Card>(target.Hand);
            }
            else
            {
                var random = new Random();
                cardsToDiscard = target.Hand.OrderBy(x => random.Next(int.MaxValue)).Take(Amount).ToList();
            }

            if (ShuffleIntoDeck)
            {
                target.Deck.ShuffleInCards(cardsToDiscard);
                
                foreach (var card in cardsToDiscard)
                {
                    target.Hand.Remove(card);
                }

                target.TriggerDiscardEvent(cardsToDiscard);
            }
            else
            {
                target.DiscardCards(cardsToDiscard);
            }
        }
    }
}
