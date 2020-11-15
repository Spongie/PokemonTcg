﻿using CardEditor.Views;
using Entities.Models;
using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class FlipCoinDamageBenchedPokemons : DataModel, IEffect
    {
        private int damage;

        [DynamicInput("Damage")]
        public int Damage
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
                return "Flip coin to damage your or opponents bench";
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
            if (game.FlipCoins(1) == 1)
            {
                foreach (var pokemon in opponent.BenchedPokemon)
                {
                    pokemon.DamageCounters += Damage;
                }
            }
            else
            {
                foreach (var pokemon in caster.BenchedPokemon)
                {
                    pokemon.DamageCounters += Damage;
                }
            }
        }
    }
}