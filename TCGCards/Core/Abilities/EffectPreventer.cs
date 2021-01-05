using CardEditor.Views;

namespace TCGCards.Core.Abilities
{
    public class EffectPreventer : PassiveAbility
    {
        private bool stopsDiscardingEnergy = true;
        
        public EffectPreventer() : this(null)
        {

        }
        public EffectPreventer(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
        }

        [DynamicInput("Stop energy discard", InputControl.Boolean)]
        public bool StopsDiscardingEnergy
        {
            get { return stopsDiscardingEnergy; }
            set
            {
                stopsDiscardingEnergy = value;
                FirePropertyChanged();
            }
        }

    }
}