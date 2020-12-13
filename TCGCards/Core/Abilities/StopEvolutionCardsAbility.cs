using CardEditor.Views;

namespace TCGCards.Core.Abilities
{
    public class StopEvolutionCardsAbility : PassiveAbility
    {
        private bool onlyWhenActive;

        public StopEvolutionCardsAbility() : this(null)
        {
            
        }

        public StopEvolutionCardsAbility(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            ModifierType = PassiveModifierType.StopEvolutions;
        }

        [DynamicInput("Only when active", InputControl.Boolean)]
        public bool OnlyWhenActive
        {
            get { return onlyWhenActive; }
            set
            {
                onlyWhenActive = value;
                FirePropertyChanged();
            }
        }

        public virtual bool IsActive()
        {
            if (onlyWhenActive)
            {
                return PokemonOwner.Owner.ActivePokemonCard.Id.Equals(PokemonOwner.Id);
            }

            return true;
        }
    }
}
