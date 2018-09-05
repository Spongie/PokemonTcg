using System.Collections.Generic;
using System.Linq;
using TCGCards.Core;

namespace TCGCards
{
    public class PokemonCard : Card
    {
        public PokemonCard(Player owner) : base(owner)
        {
            AttachedEnergy = new List<EnergyCard>();
        }

        public int Hp { get; protected set; }
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

        public void DiscardEnergyCard(EnergyCard energyCard)
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

        public PokemonCard Evolve(PokemonCard evolution)
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
