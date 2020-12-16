using CardEditor.Views;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class HealAllEffect : DataModel, IEffect
    {
        public string EffectType
        {
            get
            {
                return "Heal all your pokemon";
            }
        }

        private int amount;

        [DynamicInput("Amount")]
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                FirePropertyChanged();
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
            foreach (var pokemon in caster.GetAllPokemonCards())
            {
                pokemon.Heal(Amount, game);
            }
        }
    }
}
