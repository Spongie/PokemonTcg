using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class CreateAbilityOnGameEffect : DataModel, IEffect
    {
        public string EffectType
        {
            get
            {
                return "Create Game Ability";
            }
        }

        private Ability ability;

        [DynamicInput("Set Ability", InputControl.Ability)]
        public Ability Ability
        {
            get { return ability; }
            set
            {
                ability = value;
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
            var ability = Ability.Clone();
            ability.Source = game.CurrentTrainerCard;
            ability.IsBuff = true;
            game.TemporaryPassiveAbilities.Add(ability);
        }
    }
}
