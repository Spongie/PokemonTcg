using Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Abilities;
using TCGCards.Core.GameEvents;
using TCGCards.Core.SpecialAbilities;

namespace TCGCards
{
    public class PokemonCard : Card
    {
        private int hp;
        private EnergyTypes type;
        private int retreatCost;
        private EnergyTypes weakness;
        private EnergyTypes resistance;
        private string evolvesFrom;
        private int stage;
        private ObservableCollection<Attack> attacks = new ObservableCollection<Attack>();
        private Attack selectedAttack;
        private Ability ablity;
        private int prizeCards = 1;

        public int PrizeCards
        {
            get { return prizeCards; }
            set
            {
                prizeCards = value;
                FirePropertyChanged();
            }
        }

        public Ability Ability
        {
            get { return ablity; }
            set
            {
                ablity = value;
                FirePropertyChanged();
            }
        }

        public Attack SelectedAttack
        {
            get { return selectedAttack; }
            set
            {
                selectedAttack = value;
                FirePropertyChanged();
            }
        }

        public PokemonCard() :base(null)
        {
            Attacks = new ObservableCollection<Attack>();
        }

        public PokemonCard(Player owner) : base(owner)
        {
            AttachedEnergy = new List<EnergyCard>();
            TemporaryAbilities = new List<TemporaryAbility>();
            DamageStoppers = new List<DamageStopper>();
            AttackStoppers = new List<AttackStopper>();
        }

        public int Hp
        {
            get { return hp; }
            set
            {
                hp = value;
                FirePropertyChanged();
            }
        }

        public EnergyTypes Type
        {
            get { return type; }
            set
            {
                type = value;
                FirePropertyChanged();
            }
        }

        public int RetreatCost
        {
            get { return retreatCost; }
            set
            {
                retreatCost = value;
                FirePropertyChanged();
            }
        }

        public EnergyTypes Weakness
        {
            get { return weakness; }
            set
            {
                weakness = value;
                FirePropertyChanged();
            }
        }

        public EnergyTypes Resistance
        {
            get { return resistance; }
            set
            {
                resistance = value;
                FirePropertyChanged();
            }
        }

        public string EvolvesFrom
        {
            get { return evolvesFrom; }
            set
            {
                evolvesFrom = value;
                FirePropertyChanged();
            }
        }

        public int Stage
        {
            get { return stage; }
            set
            {
                stage = value;
                FirePropertyChanged();
            }
        }

        public ObservableCollection<Attack> Attacks
        {
            get { return attacks; }
            set
            {
                attacks = value;
                FirePropertyChanged();
            }
        }

        public int DamageCounters { get; set; }
        public PokemonCard EvolvedFrom { get; set; }
        public List<EnergyCard> AttachedEnergy { get; set; } = new List<EnergyCard>();
        public EnergyTypes PokemonType { get; set; }
        public bool PlayedThisTurn { get; set; }
        public bool IsParalyzed { get; set; }
        public bool IsBurned { get; set; }
        public bool IsPoisoned { get; set; }
        public bool IsAsleep { get; set; }
        public bool IsConfused { get; set; }
        public PokemonCard KnockedOutBy { get; set; }
        public List<TemporaryAbility> TemporaryAbilities { get; set; } = new List<TemporaryAbility>();
        public string PokemonName { get; protected set; }
        public List<DamageStopper> DamageStoppers { get; set; } = new List<DamageStopper>();
        public List<AttackStopper> AttackStoppers { get; set; } = new List<AttackStopper>();
        public int DamageTakenLastTurn { get; set; }
        public bool DoublePoison { get; set; }
        public bool EvolvedThisTurn { get; set; }
        public bool AbilityDisabled { get; set; }

        public int GetEnergyOfType(EnergyTypes energyType) => AttachedEnergy.Count(e => e.EnergyType == energyType || e.EnergyType == EnergyTypes.All);

        public virtual void EndTurn(GameField game)
        {
            TemporaryAbilities.ForEach(x => x.TurnsLeft--);
            TemporaryAbilities = TemporaryAbilities.Where(x => x.TurnsLeft > 0).ToList();
            foreach (var attack in Attacks)
            {
                attack.DamageModifier?.ReduceTurnCount();

                if (attack.DamageModifier != null && attack.DamageModifier.TurnsLeft <= 0)
                {
                    attack.DamageModifier = null;
                }
            }

            PlayedThisTurn = false;
            EvolvedThisTurn = false;

            IsParalyzed = false;

            if (IsBurned)
            {
                DealDamage(20, game, false);
                IsBurned = game.FlipCoins(1) == 0;
            }

            if (IsPoisoned)
            {
                DealDamage(DoublePoison ? 20 : 10, game, false);
            }

            if(IsAsleep)
            {
                IsAsleep = game.FlipCoins(1) == 0;
            }

            if (Ability != null)
            {
                Ability.UsedTimes = 0;
            }

            DamageStoppers.ForEach(x => x.TurnsLeft--);
            DamageStoppers = DamageStoppers.Where(x => x.TurnsLeft > 0).ToList();

            AttackStoppers.ForEach(x => x.TurnsLeft--);
            AttackStoppers = AttackStoppers.Where(x => x.TurnsLeft > 0).ToList();
        }

        public int DealDamage(Damage damage, GameField game, bool preventable = true)
        {
            var totalDamage = damage.DamageWithoutResistAndWeakness + damage.NormalDamage;

            foreach (var damageStopper in DamageStoppers)
            {
                if (preventable && damageStopper.IsDamageIgnored(totalDamage))
                {
                    totalDamage -= damageStopper.Amount;

                    if (totalDamage <= 0)
                    {
                        game.GameLog.AddMessage(GetName() + " Takes no damage");
                        return 0;
                    }
                    else
                    {
                        game.GameLog.AddMessage(GetName() + $" Takes {damageStopper.Amount} less damage");
                    }
                }
            }

            if (totalDamage > 0)
            {
                game?.SendEventToPlayers(new DamageTakenEvent() { Damage = totalDamage, PokemonId = Id });
            }

            DamageCounters += totalDamage;

            DamageTakenLastTurn = totalDamage;

            game?.GameLog.AddMessage(GetName() + $"Takes {totalDamage} damage");

            return totalDamage;
        }

        public void AttachEnergy(EnergyCard energyCard, GameField game)
        {
            energyCard.IsRevealed = true;
            AttachedEnergy.Add(energyCard);
            bool fromHand = false;

            if (Owner.Hand.Contains(energyCard))
            {
                Owner.Hand.Remove(energyCard);
                fromHand = true;
            }

            game.SendEventToPlayers(new EnergyCardsAttachedEvent()
            {
                AttachedTo = this,
                EnergyCard = energyCard
            });

            energyCard.OnAttached(this, fromHand, game);
        }

        public void DiscardEnergyCard(EnergyCard energyCard)
        {
            if (energyCard != null)
            {
                AttachedEnergy.Remove(energyCard);
                Owner.DiscardPile.Add(energyCard);
                energyCard.OnPutInDiscard(Owner);
            }
        }

        public void DiscardEnergyCardOfType(EnergyTypes energyType)
        {
            DiscardEnergyCard(AttachedEnergy.FirstOrDefault(e => e.EnergyType == energyType));
        }

        public override string GetName()
        {
            return PokemonName;
        }

        public PokemonCard Evolve(PokemonCard evolution)
        {
            ClearStatusEffects();
            evolution.SetBase(this);
            evolution.EvolvedFrom = this;
            evolution.AttachedEnergy = AttachedEnergy;
            AttachedEnergy.Clear();

            return evolution;
        }

        public void ClearStatusEffects()
        {
            IsParalyzed = false;
            IsBurned = false;
            IsPoisoned = false;
            IsAsleep = false;
            IsConfused = false;
        }

        public virtual bool CanReatreat()
        {
            if(!Owner.BenchedPokemon.Any())
                return false;

            return !IsParalyzed && !IsAsleep && AttachedEnergy.Count >= RetreatCost;
        }

        public bool IsDead() => DamageCounters >= Hp;

        public bool CanEvolve() => !PlayedThisTurn && !EvolvedThisTurn && Owner.TurnsTaken > 0;

        public bool CanAttack() => !IsParalyzed && !IsAsleep;

        public void SetBase(PokemonCard target)
        {
            DamageCounters = target.DamageCounters;
        }
        
        public bool CanEvolveTo(PokemonCard evolution)
        {
            return !string.IsNullOrWhiteSpace(evolution.EvolvesFrom) && evolution.EvolvesFrom == PokemonName;
        }

        public virtual void ApplyStatusEffect(StatusEffect statusEffect)
        {
            var statusPreventer = Ability as PreventStatusEffects;

            if (statusPreventer != null && statusPreventer.PreventsEffect(statusEffect))
            {
                return;
            }

            switch (statusEffect)
            {
                case StatusEffect.Sleep:
                    IsAsleep = true;
                    break;
                case StatusEffect.Poison:
                    IsPoisoned = true;
                    break;
                case StatusEffect.Paralyze:
                    IsParalyzed = true;
                    break;
                case StatusEffect.Burn:
                    IsBurned = true;
                    break;
                case StatusEffect.Confuse:
                    IsConfused = true;
                    break;
                case StatusEffect.None:
                    break;
                default:
                    break;
            }
        }
    }
}
