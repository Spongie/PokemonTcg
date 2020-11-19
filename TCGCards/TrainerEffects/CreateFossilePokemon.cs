using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using TCGCards.Core;

namespace TCGCards.TrainerEffects
{
    public class CreateFossilePokemon : DataModel, IEffect
    {
        public string EffectType
        {
            get
            {
                return "Create fossile Pokémon";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return GameField.BenchMaxSize - caster.BenchedPokemon.Count > 0;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent)
        {
            var pokemon = new FossilePokemon(game.CurrentTrainerCard);
            caster.BenchedPokemon.Add(pokemon);
        }
    }
}
