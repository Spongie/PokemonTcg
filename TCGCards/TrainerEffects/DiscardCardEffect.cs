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
        private CardType cardType = CardType.Any;
        private bool shuffleIntoDeck;
        private bool allowDiscardLess;
        private bool targetsOpponent;
        private CoinFlipConditional coinflipConditional = new CoinFlipConditional();

        [DynamicInput("Target Opponent?", InputControl.Boolean)]
        public bool TargetsOpponent
        {
            get { return targetsOpponent; }
            set
            {
                targetsOpponent = value;
                FirePropertyChanged();
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

        
        [DynamicInput("Allow discard 0", InputControl.Boolean)]
        public bool AllowDiscardLess
        {
            get { return allowDiscardLess; }
            set
            {
                allowDiscardLess = value;
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
            var availableChoices = CardUtil.GetCardsOfType(caster.Hand, CardType);

            if (amount == -1 && availableChoices.Count > 0)
            {
                return true;
            }

            if (AllowDiscardLess)
            {
                return true;
            }

            return availableChoices.Count >= amount;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            throw new System.NotImplementedException();
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            var target = TargetsOpponent ? opponent : caster;
            
            if (!CoinflipConditional.IsOk(game, caster))
            {
                return;
            }

            if (amount == -1)
            {
                var allCards = new List<Card>(target.Hand);

                game.LastDiscard = allCards.Count;

                if (ShuffleIntoDeck)
                {
                    target.Deck.ShuffleInCards(new List<Card>(target.Hand));
                    target.Hand.Clear();
                    target.TriggerDiscardEvent(allCards);
                }
                else
                {
                    target.DiscardCards(allCards);
                }
                return;
            }

            IDeckFilter[] filters = CardUtil.GetCardFilters(CardType).ToArray();

            int minAmount = AllowDiscardLess ? 0 : Amount;

            GameUtils.DiscardCardsFromHand(target, game, new DiscardCardSettings(minAmount, Amount, filters, ShuffleIntoDeck));
        }
    }
}
