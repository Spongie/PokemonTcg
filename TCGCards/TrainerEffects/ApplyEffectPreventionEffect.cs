using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;
using TCGCards.Core.Abilities;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class ApplyEffectPreventionEffect : DataModel, IEffect
    {
        private TargetingMode targetingMode;
        private bool coinFlip;
        private bool isMay;
        private bool useLastYesNo;
        private int turnDuration = Ability.UNTIL_YOUR_NEXT_TURN;

        public string EffectType
        {
            get
            {
                return "Apply effect prevention";
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

        [DynamicInput("Coin Flip", InputControl.Boolean)]
        public bool CoinFlip
        {
            get { return coinFlip; }
            set
            {
                coinFlip = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Ask Yes/No?", InputControl.Boolean)]
        public bool IsMay
        {
            get { return isMay; }
            set
            {
                isMay = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Use last Yes/No answer", InputControl.Boolean)]
        public bool UseLastYesNoAnswer
        {
            get { return useLastYesNo; }
            set
            {
                useLastYesNo = value;
                FirePropertyChanged();
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

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return true;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            if (IsMay)
            {
                if (UseLastYesNoAnswer && !game.LastYesNo)
                {
                    return;
                }
                else if (!game.AskYesNo(caster, "Apply effect?"))
                {
                    return;
                }
            }

            if (CoinFlip && game.FlipCoins(1) == 0)
            {
                return;
            }

            var target = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, pokemonSource);

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
