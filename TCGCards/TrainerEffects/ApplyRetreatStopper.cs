using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;
using TCGCards.Core.SpecialAbilities;

namespace TCGCards.TrainerEffects
{
    public class ApplyRetreatStopper : DataModel, IEffect
    {
        private TargetingMode targetingMode = TargetingMode.OpponentActive;
        private int turns = 2;
        private bool flipCoin;

        [DynamicInput("Flip Coin?", InputControl.Boolean)]
        public bool FlipCoin
        {
            get { return flipCoin; }
            set
            {
                flipCoin = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("How many turns")]
        public int Turns
        {
            get { return turns; }
            set
            {
                turns = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Targeting Mode", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode TargetingMode
        {
            get { return targetingMode; }
            set
            {
                targetingMode = value;
                FirePropertyChanged();
            }
        }

        public string EffectType
        {
            get
            {
                return "Apply retreat stopper";
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
            if (FlipCoin && game.FlipCoins(1) == 0)
            {
                return;
            }

            var target = CardUtil.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent);
            target.TemporaryAbilities.Add(new RetreatStopper(target, Turns));
        }
    }
}
