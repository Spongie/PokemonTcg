using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;
using TCGCards.Core.Abilities;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class ApplyPreventEffectsOfAttacksAndDamage : DataModel, IEffect
    {
        private int turnDuration = Ability.UNTIL_YOUR_NEXT_TURN;
        private TargetingMode targetingMode;
        private CoinFlipConditional conditional = new CoinFlipConditional();

        [DynamicInput("Condition", InputControl.Dynamic)]
        public CoinFlipConditional CoinflipConditional
        {
            get { return conditional; }
            set
            {
                conditional = value;
                FirePropertyChanged();
            }
        }

        public string EffectType
        {
            get
            {
                return "Prevent effects and damage of attack";
            }
        }

        [DynamicInput("Turn duration")]
        public int TurnDuration
        {
            get { return turnDuration; }
            set
            {
                turnDuration = value;
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
            if (CoinflipConditional.IsOk(game, caster))
            {
                return;
            }

            var target = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, pokemonSource);

            target.TemporaryAbilities.Add(new DamageTakenModifier(target)
            {
                Modifer = 999999,
                IsBuff = true
            });
            target.TemporaryAbilities.Add(new PreventStatusEffects(target)
            {
                IsBuff = true,
                PreventBurn = true,
                PreventConfuse = true,
                PreventParalyze = true,
                PreventPoison = true,
                PreventSleep = true,
                TurnDuration = TurnDuration
            });
            target.TemporaryAbilities.Add(new EffectPreventer()
            {
                TurnDuration = TurnDuration
            });
        }
    }
}
