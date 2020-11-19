using CardEditor.Views;
using Entities;
using System.Linq;

namespace TCGCards.Core.Abilities
{
    public class DealDamageWhenKnockedOut : Ability
    {
        private int damage;
        private bool coinFlip;
        private bool damageMultipliedByEnergy;
        private EnergyTypes energyType = EnergyTypes.All;

        [DynamicInput("Flips Coin?", InputControl.Boolean)]
        public bool CoinFlip
        {
            get { return coinFlip; }
            set
            {
                coinFlip = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Damage")]
        public int Damage
        {
            get { return damage; }
            set
            {
                damage = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Multiplied by energy?", InputControl.Boolean)]
        public bool DamageMultipliedByEnergy
        {
            get { return damageMultipliedByEnergy; }
            set
            {
                damageMultipliedByEnergy = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Energy type", InputControl.Dropdown, typeof(EnergyTypes))]
        public EnergyTypes EnergyType
        {
            get { return energyType; }
            set
            {
                energyType = value;
                FirePropertyChanged();
            }
        }
        public DealDamageWhenKnockedOut() :this(null)
        {

        }

        public DealDamageWhenKnockedOut(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.KilledByAttack;
        }

        protected override void Activate(Player player, Player opponent, int damageTake, GameField game)
        {
            if (coinFlip && CoinFlipper.FlipCoin())
                return;

            var damage = Damage;

            if (DamageMultipliedByEnergy)
            {
                damage *= PokemonOwner.AttachedEnergy.Count(energy => EnergyType == EnergyTypes.All || EnergyType == energy.EnergyType);
            }

            PokemonOwner.KnockedOutBy.DealDamage(new Damage(damage), game);

            if (PokemonOwner.KnockedOutBy.IsDead())
                PokemonOwner.KnockedOutBy.KnockedOutBy = PokemonOwner;
        }
    }
}
