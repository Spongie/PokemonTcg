using Entities;
using Newtonsoft.Json;
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
        private int resistanceAmount = 30;
        private string evolvesFrom;
        private int stage;
        private ObservableCollection<Attack> attacks = new ObservableCollection<Attack>();
        private Attack selectedAttack;
        private Ability ablity;
        private int prizeCards = 1;
        private string pokemonName;

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

        public int ResistanceAmount
        {
            get { return resistanceAmount; }
            set
            {
                resistanceAmount = value;
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

        [JsonIgnore]
        public Attack SelectedAttack
        {
            get { return selectedAttack; }
            set
            {
                selectedAttack = value;
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
                DealDamage(20, game, new PokemonCard() { Type = EnergyTypes.Fire }, false);
                IsBurned = game.FlipCoins(1) == 0;
            }

            if (IsPoisoned)
            {
                DealDamage(DoublePoison ? 20 : 10, game, new PokemonCard() { Type = EnergyTypes.Psychic }, false);
            }

            if(IsAsleep)
            {
                IsAsleep = game.FlipCoins(1) == 0;
            }

            if (Ability != null)
            {
                Ability.UsedTimes = 0;
                Ability.EndTurn();
            }
            
            ParalyzedThisTurn = false;

            AttachedTools = AttachedTools.Where(tool => !Owner.DiscardPile.Contains(tool)).ToList();

            foreach (var damagestopper in DamageStoppers)
            {
                if (!damagestopper.LastsUntilDamageTaken)
                {
                    damagestopper.TurnsLeft--;
                }
            }

            DamageStoppers = DamageStoppers.Where(x => x.TurnsLeft > 0).ToList();
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

        public int DealDamage(Damage damage, GameField game, PokemonCard source, bool preventable)
        {
            var totalDamage = damage.DamageWithoutResistAndWeakness + damage.NormalDamage;
            var damageStoppersToRemove = new List<DamageStopper>();

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

                    if (damageStopper.LastsUntilDamageTaken)
                    {
                        damageStoppersToRemove.Add(damageStopper);
                    }
                }
            }

            foreach (var damagestopper in damageStoppersToRemove)
            {
                DamageStoppers.Remove(damagestopper);
            }
            
            if (source != null && preventable)
            {
                foreach (var ability in source.GetAllActiveAbilities(game, Owner, game?.Players.First(x => !x.Id.Equals(Owner.Id))).OfType<IDamageDealtModifier>())
                {
                    totalDamage = ability.GetModifiedDamage(totalDamage, game);
                }
            }
            if (preventable)
            {
                foreach (var ability in GetAllActiveAbilities(game, Owner, game?.Players.First(x => !x.Id.Equals(Owner.Id))).OfType<IDamageTakenModifier>())
                {
                    totalDamage = ability.GetModifiedDamage(totalDamage, source, game);
                }
            }

            foreach (var pokemon in Owner.GetAllPokemonCards())
            {
                if (pokemon == this || !preventable)
                {
                    continue;
                }

                foreach (var ability in pokemon.GetAllActiveAbilities(game, Owner, game?.GetOpponentOf(Owner)).OfType<DamageRedirectorAbility>())
                {
                    if (totalDamage == 0)
                    {
                        continue;
                    }
                    if (ability.AskYesNo && !game.AskYesNo(Owner, $"Redirect damage to {ability.PokemonOwner.Name}"))
                    {
                        continue;
                    }

                    int damageToRedirect;

                    if (totalDamage >= ability.AmountToRedirect)
                    {
                        damageToRedirect = ability.AmountToRedirect;
                    }
                    else
                    {
                        damageToRedirect = totalDamage;
                    }

                    totalDamage -= damageToRedirect;
                    ability.PokemonOwner.DealDamage(damageToRedirect, game, ability.PokemonOwner, false);
                }
            }

            DamageCounters += totalDamage;
            DamageTakenLastTurn = totalDamage;

            game?.SendEventToPlayers(new DamageTakenEvent() { Damage = totalDamage, PokemonId = Id, DamageType = source != null ? source.Type : EnergyTypes.Colorless });
            game?.GameLog.AddMessage(GetName() + $"Takes {totalDamage} damage");

            return totalDamage;
        }

        public void AttachEnergy(EnergyCard energyCard, GameField game)
        {
            energyCard.RevealToAll();
            AttachedEnergy.Add(energyCard);
            bool fromHand = false;

            if (Owner.Hand.Contains(energyCard))
            {
                Owner.Hand.Remove(energyCard);
                fromHand = true;
            }
            else if (Owner.DiscardPile.Contains(energyCard))
            {
                Owner.DiscardPile.Remove(energyCard);
            }

            game.SendEventToPlayers(new EnergyCardsAttachedEvent()
            {
                AttachedTo = this,
                EnergyCard = energyCard
            });

            energyCard.OnAttached(this, fromHand, game);

            switch (energyCard.EnergyType)
            {
                case EnergyTypes.All:
                case EnergyTypes.Colorless:
                    game.TriggerAbilityOfType(TriggerType.EnergyAttached, this, 0, this);
                    break;
                case EnergyTypes.Water:
                    game.TriggerAbilityOfType(TriggerType.WaterAttached, this, 0, this);
                    break;
                case EnergyTypes.Fire:
                    game.TriggerAbilityOfType(TriggerType.FireAttached, this, 0, this);
                    break;
                case EnergyTypes.Grass:
                    game.TriggerAbilityOfType(TriggerType.GrassAttached, this, 0, this);
                    break;
                case EnergyTypes.Electric:
                    game.TriggerAbilityOfType(TriggerType.ElectricAttached, this, 0, this);
                    break;
                case EnergyTypes.Psychic:
                    game.TriggerAbilityOfType(TriggerType.PsychicAttached, this, 0, this);
                    break;
                case EnergyTypes.Fighting:
                    game.TriggerAbilityOfType(TriggerType.FightingAttached, this, 0, this);
                    break;
                case EnergyTypes.Darkness:
                    break;
                case EnergyTypes.Steel:
                    break;
                case EnergyTypes.Fairy:
                    break;
                case EnergyTypes.Dragon:
                    break;
                default:
                    break;
            }
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

        public virtual bool CanReatreat(GameField game)
        {
            if(Owner.BenchedPokemon.Count == 0)
                return false;

            return !IsParalyzed && !IsAsleep && AttachedEnergy.Sum(x => x.Amount) >= (RetreatCost + game.GetRetreatCostModified());
        }

        public void DiscardEnergyCard(EnergyCard energyCard, GameField game)
        {
            var preventer = TemporaryAbilities.OfType<EffectPreventer>().FirstOrDefault();

            if (preventer != null)
            {
                game.GameLog.AddMessage($"Discard energy card prevented by {preventer.Name}");
                return;
            }

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
            var areAbilitiesBlocked = game == null ? false : game.IsAbilitiesBlocked();


            if (Ability != null && game != null && !areAbilitiesBlocked && Ability.CanActivate(game, caster, opponent))
            {
                abilities.Add(Ability);
            }

            if (game != null)
            {
                abilities.AddRange(game.TemporaryPassiveAbilities);
            }

            abilities.AddRange(TemporaryAbilities.Where(x => x.CanActivate(game, caster, opponent)).ToList());

            return abilities;
        }

        public bool IsDead() => DamageCounters >= Hp;

        public bool CanEvolve() => !PlayedThisTurn && !EvolvedThisTurn && Owner.TurnsTaken > 0;

        public bool CanAttack() => !IsParalyzed && !IsAsleep && !TemporaryAbilities.OfType<PassiveAbility>().Any(x => x.ModifierType == PassiveModifierType.StopAttack);

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

        internal bool HaveStatus(StatusEffect requiredEffect)
        {
            switch (requiredEffect)
            {
                case StatusEffect.Sleep:
                    return IsAsleep;
                case StatusEffect.Poison:
                    return IsPoisoned;
                case StatusEffect.Paralyze:
                    return IsParalyzed;
                case StatusEffect.Burn:
                    return IsBurned;
                case StatusEffect.Confuse:
                    return IsConfused;
                default:
                    return true;
            }
        }
    }
}
