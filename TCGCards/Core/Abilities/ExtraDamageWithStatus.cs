using CardEditor.Views;
using Entities;
using System.Runtime.CompilerServices;

namespace TCGCards.Core.Abilities
{
    public class ExtraDamageWithStatus : Ability
    {
        private int amount;
        private StatusEffect status;

        [DynamicInput("Status", InputControl.Dropdown, typeof(StatusEffect))]
        public StatusEffect Status
        {
            get { return status; }
            set
            {
                status = value;
                FirePropertyChanged();
            }
        }


        [DynamicInput("Amount")]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                FirePropertyChanged();
            }
        }
        public ExtraDamageWithStatus() :this(null)
        {

        }

        public ExtraDamageWithStatus(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.DealsDamage;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            ((PokemonCard)target).DealDamage(new Damage(Amount), game);
        }

        public override bool CanActivate()
        {
            switch (Status)
            {
                case StatusEffect.Sleep:
                    return PokemonOwner.IsAsleep;
                case StatusEffect.Poison:
                    return PokemonOwner.IsPoisoned;
                case StatusEffect.Paralyze:
                    return PokemonOwner.IsParalyzed;
                case StatusEffect.Burn:
                    return PokemonOwner.IsBurned;
                case StatusEffect.Confuse:
                    return PokemonOwner.IsConfused;
                default:
                    return false;
            }
        }
    }
}
