using CardEditor.Views;
using Entities;
using System;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class ReducedBySelfDamage : Attack
    {
        private int reductionPerDamageCounter = 10;

        [DynamicInput("Damage reduced per damage counter")]
        public int ReductionPerDamageCounter
        {
            get { return reductionPerDamageCounter; }
            set 
            { 
                reductionPerDamageCounter = value;
                FirePropertyChanged();
            }
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            var reduction = (owner.ActivePokemonCard.DamageCounters / 10) * reductionPerDamageCounter;
            return Math.Max(0, Damage - reduction);
        }
    }
}
