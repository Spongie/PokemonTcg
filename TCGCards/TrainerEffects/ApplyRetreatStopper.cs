using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;
using TCGCards.Core.SpecialAbilities;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class ApplyRetreatStopper : DataModel, IEffect
    {
        private TargetingMode targetingMode = TargetingMode.OpponentActive;
        private int turns = Ability.UNTIL_YOUR_NEXT_TURN;
        private CoinFlipConditional coinflipConditional = new CoinFlipConditional();

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

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            if (CoinflipConditional.IsOk(game, caster))
            {
                return;
            }

            var target = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, caster.ActivePokemonCard);
            target?.TemporaryAbilities.Add(new RetreatStopper(target, Turns) { IsBuff = true });
        }
    }
}
