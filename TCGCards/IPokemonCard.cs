using System.Collections.Generic;
using System.Linq;
using TCGCards.Core;

namespace TCGCards
{
    public class IPokemonCard : ICard
    {
        public IPokemonCard(Player owner) : base(owner)
        {
            AttachedEnergy = new List<IEnergyCard>();
        }

        public int Hp { get; protected set; }
        public int DamageCounters { get; set; }
        public int Stage { get; set; }
        public string EvolvesFrom { get; set; }
        public IPokemonCard EvolvedFrom { get; set; }
        public List<IEnergyCard> AttachedEnergy { get; set; }
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
        public IPokemonCard KnockedOutBy { get; set; }
        public Ability Ability { get; protected set; }
        public string PokemonName { get; protected set; }

        public virtual void EndTurn()
        {
            PlayedThisTurn = false;

            if (IsBurned)
            {
                DamageCounters += 20;
                IsBurned = CoinFlipper.FlipCoin();
            }

            if (IsPoisoned)
            {
                DamageCounters += 10;
            }

            if(IsAsleep)
                IsAsleep = CoinFlipper.FlipCoin();
        }

        public void DiscardEnergyCard(IEnergyCard energyCard)
        {
            if (energyCard != null)
            {
                AttachedEnergy.Remove(energyCard);
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

        public IPokemonCard Evolve(IPokemonCard evolution)
        {
            ClearStatusEffects();
            evolution.SetBase(this);
            evolution.EvolvedFrom = this;

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

        public bool CanEvolve() => !PlayedThisTurn;

        public bool CanAttack() => !IsParalyzed && !IsAsleep;

        public void SetBase(IPokemonCard target)
        {
            DamageCounters = target.DamageCounters;
        }
        
        public bool CanEvolveTo(IPokemonCard evolution)
        {
            return !string.IsNullOrWhiteSpace(evolution.EvolvesFrom) && evolution.EvolvesFrom == PokemonName;
        }
    }
}
