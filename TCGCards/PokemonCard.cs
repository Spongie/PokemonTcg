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
        private string pokemonName;

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
            TemporaryAbilities = new List<Ability>();
            DamageStoppers = new List<DamageStopper>();
        }

        public void ReInitLists()
        {
            AttachedEnergy = new List<EnergyCard>();
            TemporaryAbilities = new List<Ability>();
            DamageStoppers = new List<DamageStopper>();
            AttachedEnergy = new List<EnergyCard>();
            TemporaryAbilities = new List<Ability>();
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

        public string PokemonName
        {
            get { return pokemonName; }
            set
            {
                pokemonName = value;
                FirePropertyChanged();
            }
        }


        public int DamageCounters { get; set; }
        public PokemonCard EvolvedFrom { get; set; }
        public List<EnergyCard> AttachedEnergy { get; set; } = new List<EnergyCard>();
        public List<TrainerCard> AttachedTools { get; set; } = new List<TrainerCard>();
        public bool PlayedThisTurn { get; set; }
        public bool ParalyzedThisTurn { get; set; }
        public bool IsParalyzed { get; set; }
        public bool IsBurned { get; set; }
        public bool IsPoisoned { get; set; }
        public bool IsAsleep { get; set; }
        public bool IsConfused { get; set; }
        public PokemonCard KnockedOutBy { get; set; }
        public List<Ability> TemporaryAbilities { get; set; } = new List<Ability>();
        public List<DamageStopper> DamageStoppers { get; set; } = new List<DamageStopper>();
        public int DamageTakenLastTurn { get; set; }
        public bool DoublePoison { get; set; }
        public bool EvolvedThisTurn { get; set; }
        public bool AbilityDisabled { get; set; }

        public int GetEnergyOfType(EnergyTypes energyType) => AttachedEnergy.Count(e => e.EnergyType == energyType || e.EnergyType == EnergyTypes.All);

        public virtual void EndTurn(GameField game)
        {
            if (TemporaryAbilities == null)
            {
                TemporaryAbilities = new List<Ability>();
            }

            foreach (var ability in TemporaryAbilities)
            {
                ability.EndTurn();

                if (ability.TurnDuration <= 0)
                {
                    ability.OnDestroyed(game);
                }
            }

            TemporaryAbilities = TemporaryAbilities.Where(x => x.TurnDuration > 0).ToList();
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

            if (!ParalyzedThisTurn)
            {
                IsParalyzed = false;
            }

            if (IsBurned)
            {
                DealDamage(20, game, this, false, false);
                IsBurned = game.FlipCoins(1) == 0;
            }

            if (IsPoisoned)
            {
                DealDamage(DoublePoison ? 20 : 10, game, this, false, false);
            }

            if(IsAsleep)
            {
                IsAsleep = game.FlipCoins(1) == 0;
            }

            if (Ability != null)
            {
                Ability.UsedTimes = 0;
            }
            
            ParalyzedThisTurn = false;

            AttachedTools = AttachedTools.Where(tool => !Owner.DiscardPile.Contains(tool)).ToList();

            DamageStoppers.ForEach(x => x.TurnsLeft--);
            DamageStoppers = DamageStoppers.Where(x => x.TurnsLeft > 0).ToList();
            Ability?.EndTurn();
        }

        public void Heal(int amount, GameField game)
        {
            DamageCounters -= amount;

            if (DamageCounters < 0)
            {
                DamageCounters = 0;
            }

            game.SendEventToPlayers(new PokemonHealedEvent
            {
                PokemonId = Id,
                Healing = amount
            });
        }

        public int DealDamage(Damage damage, GameField game, PokemonCard source, bool preventable, bool fromAttack)
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
                        game?.SendEventToPlayers(new DamageTakenEvent() { Damage = 0, PokemonId = Id, DamageType = source != null ? source.Type : EnergyTypes.Colorless });
                        return 0;
                    }
                    else
                    {
                        game.GameLog.AddMessage(GetName() + $" Takes {damageStopper.Amount} less damage");
                    }
                }
            }
            if (source != null)
            {
                foreach (var ability in source.GetAllActiveAbilities(game, Owner, game?.Players.First(x => !x.Id.Equals(Owner.Id))).OfType<IDamageDealtModifier>())
                {
                    totalDamage = ability.GetModifiedDamage(totalDamage, game);
                }
            }
            foreach (var ability in GetAllActiveAbilities(game, Owner, game?.Players.First(x => !x.Id.Equals(Owner.Id))).OfType<IDamageTakenModifier>())
            {
                totalDamage = ability.GetModifiedDamage(totalDamage, game);
            }

            DamageCounters += totalDamage;
            DamageTakenLastTurn = totalDamage;

            game?.SendEventToPlayers(new DamageTakenEvent() { Damage = totalDamage, PokemonId = Id, DamageType = source != null ? source.Type : EnergyTypes.Colorless });
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

        public override string GetName()
        {
            return Name;
        }

        public PokemonCard Evolve(PokemonCard evolution)
        {
            ClearStatusEffects();
            evolution.SetBase(this);
            evolution.EvolvedFrom = this;
            evolution.AttachedEnergy = new List<EnergyCard>(AttachedEnergy);
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

            return !IsParalyzed && !IsAsleep && AttachedEnergy.Sum(x => x.Amount) >= RetreatCost;
        }

        public void DiscardEnergyCard(EnergyCard energyCard, GameField game)
        {
            AttachedEnergy.Remove(energyCard);
            Owner.DiscardPile.Add(energyCard);
            
            game?.SendEventToPlayers(new AttachedEnergyDiscardedEvent
            {
                FromPokemonId = Id,
                DiscardedCard = energyCard
            });

            energyCard.OnPutInDiscard(Owner);
        }

        public List<Ability> GetAllActiveAbilities(GameField game, Player caster, Player opponent)
        {
            var abilities = new List<Ability>();
            
            if (Ability != null && game != null && Ability.CanActivate(game, caster, opponent))
            {
                abilities.Add(Ability);
            }

            abilities.AddRange(TemporaryAbilities.Where(x => x.CanActivate(game, caster, opponent)).ToList());

            return abilities;
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

        public virtual void ApplyStatusEffect(StatusEffect statusEffect, GameField game)
        {
            var statusPreventer = Ability as IStatusPreventer;

            if (statusPreventer != null && statusPreventer.PreventsEffect(statusEffect, game))
            {
                return;
            }

            switch (statusEffect)
            {
                case StatusEffect.Sleep:
                    IsAsleep = true;
                    IsConfused = false;
                    IsParalyzed = false;
                    break;
                case StatusEffect.Poison:
                    IsPoisoned = true;
                    break;
                case StatusEffect.Paralyze:
                    ParalyzedThisTurn = true;
                    IsParalyzed = true;
                    IsAsleep = false;
                    IsConfused = false;
                    break;
                case StatusEffect.Burn:
                    IsBurned = true;
                    break;
                case StatusEffect.Confuse:
                    IsConfused = true;
                    IsAsleep = false;
                    IsParalyzed = false;
                    break;
                case StatusEffect.None:
                    break;
                default:
                    break;
            }
        }

        public virtual void OnDeath() { }
    }
}
