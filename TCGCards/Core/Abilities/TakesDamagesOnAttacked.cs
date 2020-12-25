using CardEditor.Views;
using Entities;

namespace TCGCards.Core.Abilities
{
    public class TakesDamagesOnAttacked : Ability
    {
        private int damageReturned;
        private bool attackBack;
        private float attackBackModifier = 1.0f;
        private bool coinFlip;
        private StatusEffect requiredEffect = StatusEffect.None;

        public TakesDamagesOnAttacked() :this(null)
        {

        }

        public TakesDamagesOnAttacked(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.TakesDamage;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            if (CoinFlip && game.FlipCoins(1) == 0)
            {
                return;
            }
            
            if (requiredEffect != StatusEffect.None && !PokemonOwner.HaveStatus(RequiredEffect))
            {
                return;
            }

            if (attackBack)
            {
                var baseDamage = DamageReturned > 0 ? DamageReturned : damageTaken;
                var damage = DamageCalculator.GetDamageAfterWeaknessAndResistance((int)(baseDamage * attackBackModifier), PokemonOwner, opponent.ActivePokemonCard, null);
                opponent.ActivePokemonCard.DealDamage(damage, game, PokemonOwner, true);
            }
            {
                opponent.ActivePokemonCard.DealDamage(DamageReturned, game, PokemonOwner, false);
            }
        }

        [DynamicInput("Require effect on self", InputControl.Dropdown, typeof(StatusEffect))]
        public StatusEffect RequiredEffect
        {
            get { return requiredEffect; }
            set
            {
                requiredEffect = value;
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


        [DynamicInput("Attack back", InputControl.Boolean)]
        public bool AttackBack
        {
            get { return attackBack; }
            set
            {
                attackBack = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Modifer if attacking back")]
        public float AttackBackModifier
        {
            get { return attackBackModifier; }
            set
            {
                attackBackModifier = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Damage returned")]
        public int DamageReturned
        {
            get { return damageReturned; }
            set
            {
                damageReturned = value;
                FirePropertyChanged();
            }
        }

    }
}
