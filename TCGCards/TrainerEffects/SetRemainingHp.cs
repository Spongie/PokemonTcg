using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class SetRemainingHp : DataModel, IEffect
    {
        private bool onlyIfDead;
        private int remainingHp;
        private CoinFlipConditional coinFlipConditional = new CoinFlipConditional();
        private TargetingMode targeting = TargetingMode.Self;

        public string EffectType
        {
            get
            {
                return "Set remaining HP";
            }
        }

        [DynamicInput("Target", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode TargetMode
        {
            get { return targeting; }
            set
            {
                targeting = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Condition", InputControl.Dynamic)]
        public CoinFlipConditional CoinFlipConditional
        {
            get { return coinFlipConditional; }
            set
            {
                coinFlipConditional = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Only if dead", InputControl.Boolean)]
        public bool OnlyIfDead
        {
            get { return onlyIfDead; }
            set
            {
                onlyIfDead = value;
                FirePropertyChanged();
            }
        }
        
        [DynamicInput("Remaining HP")]
        public int RemainingHp
        {
            get { return remainingHp; }
            set
            {
                remainingHp = value;
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
            var target = Targeting.AskForTargetFromTargetingMode(TargetMode, game, caster, opponent, pokemonSource);

            if (OnlyIfDead && !target.IsDead())
            {
                return;
            }

            target.DamageCounters = target.Hp - RemainingHp;
        }
    }
}
