using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;
using TCGCards.Core.Abilities;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class ApplyAttackStopperEffect : DataModel, IEffect
    {
        private TargetingMode targetingMode;
        private int duration;
        private bool onlyStopAttacksOnSelf;
        private string onlyThisAttack;

        public string EffectType
        {
            get
            {
                return "Apply attack stopper";
            }
        }

        [DynamicInput("Duration")]
        public int Duration
        {
            get { return duration; }
            set
            {
                duration = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Target", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode TargetingMode
        {
            get { return targetingMode; }
            set
            {
                targetingMode = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Only stop attacking self", InputControl.Boolean)]
        public bool OnlyStopAttacksOnSelf
        {
            get { return onlyStopAttacksOnSelf; }
            set
            {
                onlyStopAttacksOnSelf = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Only stop this attack")]
        public string OnlyThisAttack
        {
            get { return onlyThisAttack; }
            set
            {
                onlyThisAttack = value;
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
            var target = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, pokemonSource);

            var ability = new AttackStoppingAbility()
            {
                TurnDuration = Duration,
                IsBuff = true,
                OnlyStopThisAttack = OnlyThisAttack
            };

            if (OnlyStopAttacksOnSelf)
            {
                ability.OnlyOnCard = pokemonSource.Id;
            }

            target.TemporaryAbilities.Add(ability);
        }
    }
}
