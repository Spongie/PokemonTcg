using System.Collections.Generic;
using TCGCards.Core;
using System.Linq;
using NetworkingCore;
using TCGCards.Core.Abilities;

namespace TCGCards
{
    public abstract class Attack
    {
        public Attack()
        {
            Description = string.Empty;
            Id = NetworkId.Generate();
        }

        public string DamageText { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<Energy> Cost { get; set; }

        public bool ApplyWeaknessAndResistance { get; set; } = true;

        public NetworkId Id { get; set; }
        public bool Disabled { get; set; }

        public virtual void PayExtraCosts(GameField game, Player owner, Player opponent) { }

        public abstract Damage GetDamage(Player owner, Player opponent);

        public virtual void ProcessEffects(GameField game, Player owner, Player opponent) { }

        public virtual void OnDamageDealt(int amount, Player owner) { }

        public virtual bool CanBeUsed(GameField game, Player owner, Player opponent)
        {
            if (game.FirstTurn)
            {
                return false;
            }

            var availableEnergy = new List<EnergyCard>(owner.ActivePokemonCard.AttachedEnergy).OrderBy(card => card.IsBasic).ToList();
            var energyOverride = owner.ActivePokemonCard.TemporaryAbilities.OfType<EnergyTypeOverrideTemporaryAbility>().FirstOrDefault();

            foreach (var energy in Cost.OrderByDescending(cost => cost.EnergyType != EnergyTypes.Colorless))
            {
                for (int i = 0; i < energy.Amount; i+=0)
                {
                    var actualType = energy.EnergyType;

                    if (energyOverride != null && energyOverride.SourceTypes.Contains(energy.EnergyType))
                    {
                        actualType = energyOverride.NewType;
                    }

                    EnergyCard energyCard = actualType == EnergyTypes.Colorless ?
                        availableEnergy.FirstOrDefault()
                        : availableEnergy.FirstOrDefault(card => card.EnergyType == actualType);

                    if (energyCard == null)
                    {                 
                        return false;
                    }

                    availableEnergy.Remove(energyCard);

                    i += energyCard.GetEnergry().Amount;
                }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Attack;

            if (other == null)
            {
                return false;
            }

            return other.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            var hashCode = 927195835;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Energy>>.Default.GetHashCode(Cost);
            hashCode = hashCode * -1521134295 + ApplyWeaknessAndResistance.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<NetworkId>.Default.GetHashCode(Id);
            return hashCode;
        }
    }
}