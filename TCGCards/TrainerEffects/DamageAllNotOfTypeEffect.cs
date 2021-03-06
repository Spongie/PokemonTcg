﻿using CardEditor.Views;
using Entities;
using Entities.Models;
using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class DamageAllNotOfTypeEffect : DataModel, IEffect
    {
        private int teamBenchDamage;
        private int enemyBenchDamage;
        private EnergyTypes notOfType;

        [DynamicInput("Damage all not of type", InputControl.Dropdown, typeof(EnergyTypes))]
        public EnergyTypes NotOfType
        {
            get { return notOfType; }
            set
            {
                notOfType = value;
                FirePropertyChanged();
            }
        }


        [DynamicInput("Damage to your bench")]
        public int TeamBenchDamage
        {
            get { return teamBenchDamage; }
            set
            {
                teamBenchDamage = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Damage to opponents bench")]
        public int EnemyBenchDamage
        {
            get { return enemyBenchDamage; }
            set
            {
                enemyBenchDamage = value;
                FirePropertyChanged();
            }
        }

        public string EffectType
        {
            get
            {
                return "Damage multiple not of type";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return true;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {

        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            if (EnemyBenchDamage > 0)
            {
                foreach (var pokemon in opponent.BenchedPokemon.ValidPokemonCards)
                {
                    if (pokemon.Type != NotOfType)
                    {
                        pokemon.DealDamage(EnemyBenchDamage, game, pokemonSource, true);
                    }
                }
            }
            if (TeamBenchDamage > 0)
            {
                foreach (var pokemon in caster.BenchedPokemon.ValidPokemonCards)
                {
                    if (pokemon.Type != NotOfType)
                    {
                        pokemon.DealDamage(TeamBenchDamage, game, pokemonSource, true);
                    }
                }
            }
        }
    }
}