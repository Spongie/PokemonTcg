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

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            if (FlipCoin && game.FlipCoins(1) == 0)
            {
                return;
            }

            var target = Targeting.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, caster.ActivePokemonCard);
            target?.TemporaryAbilities.Add(new RetreatStopper(target, Turns));
        }
    }
}
