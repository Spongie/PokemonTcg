﻿using Entities.Models;
using TCGCards.Core;
using TCGCards.Core.Abilities;

namespace TCGCards.TrainerEffects
{
    public class ApplyStopTrainerCards : DataModel, IEffect
    {
        public string EffectType
        {
            get
            {
                return "Stop trainer casting";
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
            game.TemporaryPassiveAbilities.Add(new StopTrainerCastsAbility(caster.ActivePokemonCard)
            {
                LimitedByTime = true,
                TurnsLeft = 2,
                IsBuff = true
            });
        }
    }
}
