using CardEditor.Views;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using TCGCards.Core;
using TCGCards.Core.SpecialAbilities;

namespace TCGCards.Attacks
{
    public class ApplyDamagePreventionEffect : Attack
    {
        private int amount;
        private bool coinFlip;

        [DynamicInput("Coin flipped", InputControl.Boolean)]
        public bool CoinFlip
        {
            get { return coinFlip; }
            set 
            { 
                coinFlip = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Damage to prevent")]
        public int Amount
        {
            get { return amount; }
            set 
            { 
                amount = value;
                FirePropertyChanged();
            }
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (CoinFlip)
            {
                if (game.FlipCoins(1) == 0)
                {
                    return;
                }
            }

            owner.ActivePokemonCard.DamageStoppers.Add(new DamageStopper(() => true) { Amount = amount });
        }
    }
}
