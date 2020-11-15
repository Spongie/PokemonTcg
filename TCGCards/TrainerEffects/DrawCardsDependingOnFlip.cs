using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class DrawCardsDependingOnFlip : DataModel, IEffect
    {
        private int amountIfHeads;
        private int amountIfTails;

        [DynamicInput("Amount if Tails")]
        public int AmountIfTails
        {
            get { return amountIfTails; }
            set
            {
                amountIfTails = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Amount if Heads")]
        public int AmountIfHeads
        {
            get { return amountIfHeads; }
            set
            {
                amountIfHeads = value;
                FirePropertyChanged();
            }
        }

        public string EffectType
        {
            get
            {
                return "Draw cards depending on flip";
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
            if (game.FlipCoins(1) == 1)
            {
                caster.DrawCards(AmountIfHeads);
            }
            else
            {
                caster.DrawCards(AmountIfTails);
            }
        }
    }
}
