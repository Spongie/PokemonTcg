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

            target.DrawCards(Amount);
        }
    }
}
