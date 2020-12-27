using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class CardDrawEffect : DataModel, IEffect
    {
        private int amount;
        private bool onlyOnCoinflip;
        private bool opponents;
        private bool isMay;
        private bool useLastDiscard;
        private float modiferForLastCount = 1.0f;

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

        [DynamicInput("Cards to draw")]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Ask yes/no", InputControl.Boolean)]
        public bool IsMay
        {
            get { return isMay; }
            set
            {
                isMay = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Use last discarded amount", InputControl.Boolean)]
        public bool UseLastDiscardCount
        {
            get { return useLastDiscard; }
            set
            {
                useLastDiscard = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Last discard count modifier")]
        public float ModifierForLastDiscardCount
        {
            get { return modiferForLastCount; }
            set
            {
                modiferForLastCount = value;
                FirePropertyChanged();
            }
        }


        public string EffectType
        {
            get
            {
                return "Draw cards";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent) => true;

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            attachedTo.Owner.DrawCards(Amount);
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            var target = opponents ? opponent : caster;

            if (IsMay && !game.AskYesNo(target, $"Draw {Amount} Cards?"))
            {
                return;
            }
            
            if (onlyOnCoinflip && game.FlipCoins(1) == 0)
            {
                return;
            }

            int amount = UseLastDiscardCount ? (int)(game.LastDiscard * ModifierForLastDiscardCount) : Amount;

            target.DrawCards(amount);
        }
    }
}
