using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class DamageEffect : DataModel, IEffect
    {
        private int amount;
        private TargetingMode targetingMode = TargetingMode.OpponentActive;

        [DynamicInput("Damage amount")]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Target?", InputControl.Dropdown, typeof(TargetingMode))]
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
                return "Damage";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent) => true;

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            attachedTo.DealDamage(Amount, game, attachedTo);
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            var target = CardUtil.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, caster.ActivePokemonCard);

            target.DealDamage(Amount, game, pokemonSource);
        }
    }
}
