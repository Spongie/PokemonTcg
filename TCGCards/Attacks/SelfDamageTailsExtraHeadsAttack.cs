using CardEditor.Views;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class SelfDamageTailsExtraHeadsAttack : Attack
    {
        private int selfDamage;
        private int extraDamage;

        [DynamicInput("SelfDamage when tails")]
        public int SelfDamage
        {
            get { return selfDamage; }
            set
            {
                selfDamage = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Extra Damage when heads")]
        public int ExtraDamage
        {
            get { return extraDamage; }
            set
            {
                extraDamage = value;
                FirePropertyChanged();
            }
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            if (game.FlipCoins(1) == 0)
            {
                owner.ActivePokemonCard.DamageCounters += SelfDamage;
                return Damage;
            }
            else
            {
                return Damage + ExtraDamage;
            }
        }
    }
}
