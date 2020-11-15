﻿using CardEditor.Views;
using Entities.Models;
using System;
using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class Thunderstorm : DataModel, IEffect
    {
        private int damage;
        private int damagePerTails;

        [DynamicInput("Damage to self per tails")]
        public int DamagePerTails
        {
            get { return damagePerTails; }
            set
            {
                damagePerTails = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Damage to opponents")]
        public int DamageToOpponents
        {
            get { return damage; }
            set
            {
                damage = value;
                FirePropertyChanged();
            }
        }

        public string EffectType
        {
            get
            {
                return "Thunderstorm";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return true;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent)
        {
            var tails = 0;

            foreach (var pokemon in opponent.BenchedPokemon)
            {
                if (game.FlipCoins(1) == 1)
                {
                    pokemon.DamageCounters += DamageToOpponents;
                }
                else
                {
                    tails++;
                }
            }

            caster.ActivePokemonCard.DamageCounters += tails * damagePerTails;
        }
    }
}