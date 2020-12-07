using CardEditor.Views;
using Entities;

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
            if (target != null)
            {
                ((PokemonCard)target).DealDamage(new Damage(Amount), game, PokemonOwner, true, true);
            }
            else
            {
                opponent.ActivePokemonCard.DealDamage(new Damage(Amount), game, PokemonOwner, true, true);
            }
        }

        public override bool CanActivate(GameField game, Player caster, Player opponent)
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
