using CardEditor.Views;
using Entities;

namespace TCGCards.Core.Abilities
{
    public class PreventStatusEffects : PassiveAbility
    {
        private bool preventBurn;
        private bool preventConfuse;
        private bool preventSleep;
        private bool preventParalyze;
        private bool preventPoison;

        [DynamicInput("Prevent Poison")]
        public bool PreventPoison
        {
            get { return preventPoison; }
            set
            {
                preventPoison = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Prevent Paralyze")]
        public bool PreventParalyze
        {
            get { return preventParalyze; }
            set
            {
                preventParalyze = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Prevent Sleep")]
        public bool PreventSleep
        {
            get { return preventSleep; }
            set
            {
                preventSleep = value;
                FirePropertyChanged();
            }
        }


        [DynamicInput("Prevent Confuse")]
        public bool PreventConfuse
        {
            get { return preventConfuse; }
            set
            {
                preventConfuse = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Prevent Burn")]
        public bool PreventBurn
        {
            get { return preventBurn; }
            set
            {
                preventBurn = value;
                FirePropertyChanged();
            }
        }

        public PreventStatusEffects() :this(null)
        {

        }

        public PreventStatusEffects(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            ModifierType = PassiveModifierType.StopStatusEffects;
        }

        public bool PreventsEffect(StatusEffect statusEffect)
        {
            switch (statusEffect)
            {
                case StatusEffect.Sleep:
                    return preventSleep;
                case StatusEffect.Poison:
                    return preventPoison;
                case StatusEffect.Paralyze:
                    return preventParalyze;
                case StatusEffect.Burn:
                    return preventBurn;
                case StatusEffect.Confuse:
                    return preventConfuse;
                case StatusEffect.None:
                default:
                    return false;
            }
        }
    }
}