using Entities;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.SpecialAbilities;

namespace TCGCards
{
    public class PokemonCard : Card
    {
        public PokemonCard(Player owner) : base(owner)
        {
            AttachedEnergy = new List<EnergyCard>();
            TemporaryAbilities = new List<TemporaryAbility>();
            DamageStoppers = new List<DamageStopper>();
            AttackStoppers = new List<AttackStopper>();
        }

        public int Hp { get; set; }
        public int DamageCounters { get; set; }
        public int Stage { get; set; }
        public string EvolvesFrom { get; set; }
        public PokemonCard EvolvedFrom { get; set; }
        public List<EnergyCard> AttachedEnergy { get; set; }
        public EnergyTypes PokemonType { get; set; }
        public EnergyTypes Resistance { get; set; }
        public EnergyTypes Weakness { get; set; }
        public List<Attack> Attacks { get; set; }
        public int RetreatCost { get; set; }
        public bool PlayedThisTurn { get; set; }
        public bool IsParalyzed { get; set; }
        public bool IsBurned { get; set; }
        public bool IsPoisoned { get; set; }
        public bool IsAsleep { get; set; }
        public bool IsConfused { get; set; }
        public PokemonCard KnockedOutBy { get; set; }
        public Ability Ability { get; set; }
        public List<TemporaryAbility> TemporaryAbilities { get; set; }
        public string PokemonName { get; protected set; }
        public List<DamageStopper> DamageStoppers { get; set; }
        public List<AttackStopper> AttackStoppers { get; set; }
        public int DamageTakenLastTurn { get; set; }
        public bool DoublePoison { get; set; }
        public bool EvolvedThisTurn { get; set; }
        public bool AbilityDisabled { get; set; }

        public int GetEnergyOfType(EnergyTypes energyType) => AttachedEnergy.Count(e => e.EnergyType == energyType || e.EnergyType == EnergyTypes.All);

        public virtual void EndTurn()
        {
            TemporaryAbilities.ForEach(x => x.TurnsLeft--);
            TemporaryAbilities = TemporaryAbilities.Where(x => x.TurnsLeft > 0).ToList();

            PlayedThisTurn = false;
            EvolvedThisTurn = false;

            if (IsBurned)
            {
                DamageCounters += 20;
                IsBurned = CoinFlipper.FlipCoin();
            }

            if (IsPoisoned)
            {
                DamageCounters += DoublePoison ? 20 : 10;
            }

            if(IsAsleep)
            {
                IsAsleep = CoinFlipper.FlipCoin();
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

        public int DealDamage(Damage damage, GameLog log)
        {
            if (DamageStoppers.Any(x => x.IsDamageIgnored()))
            {
                log.AddMessage(GetName() + "Takes no damage");
                return 0;
            }

            var totalDamage = damage.DamageWithoutResistAndWeakness + damage.NormalDamage;

            DamageCounters += totalDamage;

            DamageTakenLastTurn = totalDamage;

            log.AddMessage(GetName() + $"Takes {totalDamage} damage");

            return totalDamage;
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

        public bool CanReatreat()
        {
            if(!Owner.BenchedPokemon.Any())
                return false;

            return !IsParalyzed && AttachedEnergy.Count >= RetreatCost;
        }

        public bool IsDead() => DamageCounters >= Hp;

        public bool CanEvolve() => !PlayedThisTurn && !EvolvedThisTurn;

        public bool CanAttack() => !IsParalyzed && !IsAsleep;

        public void SetBase(PokemonCard target)
        {
            DamageCounters = target.DamageCounters;
        }
        
        public bool CanEvolveTo(PokemonCard evolution)
        {
            return !string.IsNullOrWhiteSpace(evolution.EvolvesFrom) && evolution.EvolvesFrom == PokemonName;
        }
    }
}
