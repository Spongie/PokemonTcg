using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.TrainerEffects
{
    public class FullHealEffect : DataModel, IEffect
    {
        private TargetingMode targetingMode;
        private bool useLastTarget;

        [DynamicInput("Targeting type", InputControl.Dropdown, typeof(TargetingMode))]
        public TargetingMode TargetingMode
        {
            get { return targetingMode; }
            set
            {
                targetingMode = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Use last target", InputControl.Boolean)]
        public bool UseLastTarget
        {
            get { return useLastTarget; }
            set
            {
                useLastTarget = value;
                FirePropertyChanged();
            }
        }

        public string EffectType
        {
            get
            {
                return "Full Heal";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent) => true;

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            FullHealPokemon(attachedTo);
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            PokemonCard target = UseLastTarget ? game.LastTarget : Targeting.AskForTargetFromTargetingMode(TargetingMode, game, caster, opponent, caster.ActivePokemonCard);
            
            if (target == null)
            {
                return;
            }

            FullHealPokemon(target);
        }

        private void FullHealPokemon(PokemonCard pokemon)
        {
            pokemon.IsAsleep = false;
            pokemon.IsConfused = false;
            pokemon.IsParalyzed = false;
            pokemon.IsPoisoned = false;
            pokemon.IsBurned = false;
        }
    }
}
