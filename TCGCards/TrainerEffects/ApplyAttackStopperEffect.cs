using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;
using TCGCards.Core.Abilities;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class ApplyAttackStopperEffect : DataModel, IEffect
    {
        public string EffectType
        {
            get
            {
                return "Apply attack stopper";
            }
        }

        private TargetingMode targetingMode;
        private int duration;

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

            target.TemporaryAbilities.Add(new AttackStoppingAbility()
            {
                TurnDuration = Duration,
                IsBuff = true
            });
        }
    }
}
