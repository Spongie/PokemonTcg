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
        private bool coinFlip;

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


        [DynamicInput("Prevent Poison", InputControl.Boolean)]
        public bool PreventPoison
        {
            get { return preventPoison; }
            set
            {
                preventPoison = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Prevent Paralyze", InputControl.Boolean)]
        public bool PreventParalyze
        {
            get { return preventParalyze; }
            set
            {
                preventParalyze = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Prevent Sleep", InputControl.Boolean)]
        public bool PreventSleep
        {
            get { return preventSleep; }
            set
            {
                preventSleep = value;
                FirePropertyChanged();
            }
        }


        [DynamicInput("Prevent Confuse", InputControl.Boolean)]
        public bool PreventConfuse
        {
            get { return preventConfuse; }
            set
            {
                preventConfuse = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Prevent Burn", InputControl.Boolean)]
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
            if (CoinFlip && CoinFlipper.FlipCoin())
            {
                return false;
            }

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