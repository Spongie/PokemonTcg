using CardEditor.Views;
using Entities.Models;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class DiscardCardEffect : DataModel, IEffect
    {
        private int amount;
        private bool onlyOnCoinflip;
        private CardType cardType = CardType.Any;
        private bool shuffleIntoDeck;

        [DynamicInput("Flip coin", InputControl.Boolean)]
        public bool OnlyOnCoinFlip
        {
            get { return onlyOnCoinflip; }
            set
            {
                onlyOnCoinflip = value;
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


        public string EffectType
        {
            get
            {
                return "Discard cards";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            if (amount == -1)
            {
                return true;
            }

            return caster.Hand.Count - 1 >= amount;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            throw new System.NotImplementedException();
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            if (onlyOnCoinflip && game.FlipCoins(1) == 0)
            {
                return;
            }

            if (amount == -1)
            {
                var allCards = new List<Card>(caster.Hand);

                if (ShuffleIntoDeck)
                {
                    caster.Deck.ShuffleInCards(new List<Card>(caster.Hand));
                    caster.Hand.Clear();
                    caster.TriggerDiscardEvent(allCards);
                }
                else
                {
                    caster.DiscardCards(allCards);
                }
                return;
            }

            List<IDeckFilter> filters = CardUtil.GetCardFilters(CardType).ToList();

            GameUtils.DiscardCardsFromHand(caster, Amount, filters, ShuffleIntoDeck);
        }
    }
}
