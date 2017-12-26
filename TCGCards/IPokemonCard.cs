using System;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core;

namespace TCGCards
{
    public abstract class IPokemonCard : ICard
    {
        protected IPokemonCard(Player owner) : base(owner)
        {
            AttachedEnergy = new List<IEnergyCard>();
        }

        public int Hp { get; protected set; }
        public int DamageCounters { get; set; }
        public int Stage { get; set; }
        public IPokemonCard EvolvesFrom { get; set; }
        public List<IEnergyCard> AttachedEnergy { get; set; }
        public EnergyTypes PokemonType { get; set; }
        public EnergyTypes Resistance { get; set; }
        public EnergyTypes Weakness { get; set; }
        public List<Attack> Attacks { get; set; }
        public int RetreatCost { get; set; }
        public bool PlayedThisTurn { get; set; }

        public bool CanReatreat()
        {
            if(!Owner.BenchedPokemon.Any())
                return false;

            return AttachedEnergy.Count >= RetreatCost;
        }

        internal bool IsDead()
        {
            return DamageCounters >= Hp;
        }
    }
}
