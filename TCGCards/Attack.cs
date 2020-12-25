using System.Collections.Generic;
using TCGCards.Core;
using System.Linq;
using NetworkingCore;
using TCGCards.Core.Abilities;
using Entities;
using Entities.Models;
using System.Collections.ObjectModel;
using TCGCards.Core.Messages;
using TCGCards.TrainerEffects;
using TCGCards.Core.SpecialAbilities;

namespace TCGCards
{
    public class Attack : DataModel, IEntity
    {
        private ObservableCollection<Energy> cost = new ObservableCollection<Energy>();
        private ObservableCollection<Energy> extraDiscardCost = new ObservableCollection<Energy>();
        private ObservableCollection<IEffect> effects = new ObservableCollection<IEffect>();
        private string name = "New Attack";
        private string description;
        private int damage;
        protected bool foreverDisabled = false;
        private bool applyWeakness = true;
        private bool applyResistance = true;

        public Attack()
        {
            Description = string.Empty;
            Id = NetworkId.Generate();
        }

        public Attack(Attack selectedAttack) :this()
        {
            Description = selectedAttack.Description;
            Name = selectedAttack.Name;
            Effects = new ObservableCollection<IEffect>(selectedAttack.Effects);
            Cost = new ObservableCollection<Energy>(selectedAttack.Cost);
            ExtraDiscardCost = new ObservableCollection<Energy>(selectedAttack.ExtraDiscardCost);
            Damage = selectedAttack.Damage;
        }

        public bool ApplyWeakness
        {
            get { return applyWeakness; }
            set
            {
                applyWeakness = value;
                FirePropertyChanged();
            }
        }

        public bool ApplyResistance
        {
            get { return applyResistance; }
            set
            {
                applyResistance = value;
                FirePropertyChanged();
            }
        }

        public ObservableCollection<IEffect> Effects
        {
            get { return effects; }
            set
            {
                effects = value;
                FirePropertyChanged();
            }
        }


        public ObservableCollection<Energy> Cost
        {
            get { return cost; }
            set
            {
                cost = value;
                FirePropertyChanged();
            }
        }

        public ObservableCollection<Energy> ExtraDiscardCost
        {
            get { return extraDiscardCost; }
            set 
            {
                extraDiscardCost = value;
                FirePropertyChanged();
            }
        }

        public int Damage
        {
            get 
            { 
                if (DamageModifier != null)
                {
                    return DamageModifier.NewDamage;
                }

                return damage; 
            }
            set
            {
                damage = value;
                FirePropertyChanged();
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                FirePropertyChanged();
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                FirePropertyChanged();
            }
        }

        public string AttackType { get => GetType().Name; }
        public NetworkId Id { get; set; }
        public bool Disabled { get; set; }
        public DamageModifier DamageModifier { get; set; }

        public virtual void PayExtraCosts(GameField game, Player owner, Player opponent) 
        {
            foreach (var energy in ExtraDiscardCost)
            {
                var choices = owner.ActivePokemonCard.AttachedEnergy.Where(x => x.EnergyType == energy.EnergyType).ToList();

                if (energy.EnergyType == EnergyTypes.All && energy.Amount == -1)
                {
                    foreach (var card in new List<EnergyCard>(owner.ActivePokemonCard.AttachedEnergy))
                    {
                        owner.ActivePokemonCard.DiscardEnergyCard(card, game);
                    }

                    return;
                }
                else if (energy.EnergyType == EnergyTypes.All)
                {
                    choices = owner.ActivePokemonCard.AttachedEnergy.ToList();
                }

                var message = new PickFromListMessage(choices.OfType<Card>().ToList(), energy.Amount);
                var response = owner.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message.ToNetworkMessage(Id));

                foreach (var card in response.Cards)
                {
                    owner.ActivePokemonCard.DiscardEnergyCard((EnergyCard)game.Cards[card], game);
                }
            }
        }

        public virtual Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            return damage;
        }

        public virtual void ProcessEffects(GameField game, Player owner, Player opponent) 
        {
            foreach (var effect in Effects)
            {
                effect.Process(game, owner, opponent, owner.ActivePokemonCard);
            }
        }

        public virtual void OnDamageDealt(int amount, Player owner, GameField game) { }

        public virtual bool CanBeUsed(GameField game, Player owner, Player opponent)
        {
            if (game.FirstTurn || foreverDisabled)
            {
                return false;
            }

            foreach (var ability in owner.ActivePokemonCard.GetAllActiveAbilities(game, owner, opponent).OfType<AttackStoppingAbility>())
            {
                if (ability.OnlyOnCard != null)
                {
                    if (!string.IsNullOrEmpty(ability.OnlyStopThisAttack.Trim()))
                    {
                        return ability.OnlyStopThisAttack.Trim() != Name;
                    }
                    if (opponent.ActivePokemonCard.Id.Equals(ability.OnlyOnCard))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(ability.OnlyStopThisAttack.Trim()))
                    {
                        return ability.OnlyStopThisAttack.Trim() != Name;
                    }

                    return false;
                }
            }

            return HaveEnoughEnergy(owner) && Effects.All(x => x.CanCast(game, owner, opponent));
        }

        public bool HaveEnoughEnergy(Player owner)
        {
            var availableEnergy = new List<EnergyCard>(owner.ActivePokemonCard.AttachedEnergy).OrderBy(card => card.IsBasic).ToList();
            var energyOverride = owner.ActivePokemonCard.TemporaryAbilities.OfType<EnergyTypeOverrideTemporaryAbility>().FirstOrDefault();
            var energyCosts = new Dictionary<EnergyTypes, int>();
            
            foreach (var cost in Cost)
            {
                energyCosts.Add(cost.EnergyType, cost.Amount);
            }

            foreach (var energy in availableEnergy)
            {
                var actualType = energy.EnergyType;

                if (energyOverride != null && energyOverride.CoversType(energy.EnergyType))
                {
                    actualType = energyOverride.NewType;
                }

                if (energyCosts.ContainsKey(actualType) && energyCosts[actualType] > 0)
                {
                    energyCosts[actualType] -= energy.Amount;
                }
                else if (energyCosts.ContainsKey(EnergyTypes.Colorless))
                {
                    energyCosts[EnergyTypes.Colorless] -= energy.Amount;
                }
            }

            foreach (var cost in energyCosts)
            {
                if (cost.Value > 0)
                {
                    return false;
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
            hashCode = hashCode * -1521134295 + EqualityComparer<ObservableCollection<Energy>>.Default.GetHashCode(Cost);
            hashCode = hashCode * -1521134295 + EqualityComparer<ObservableCollection<IEffect>>.Default.GetHashCode(Effects);
            hashCode = hashCode * -1521134295 + EqualityComparer<NetworkId>.Default.GetHashCode(Id);
            return hashCode;
        }
    }
}