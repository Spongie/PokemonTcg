﻿using CardEditor.Views;
using Entities.Models;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards.TrainerEffects
{
    public class DamageToXBenched : DataModel, IEffect
    {
        private int amountOfTargets;
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

        [DynamicInput("Nr. of Targets")]
        public int AmountOfTargets
        {
            get { return amountOfTargets; }
            set
            {
                amountOfTargets = value;
                FirePropertyChanged();
            }
        }

        public string EffectType
        {
            get
            {
                return "Deals damage to multiple benched";
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
            if (opponent.BenchedPokemon.Count <= AmountOfTargets)
            {
                foreach (var pokemon in opponent.BenchedPokemon)
                {
                    pokemon.DamageCounters += Damage;
                }

                return;
            }

            var message = new SelectFromOpponentBenchMessage(0, 3).ToNetworkMessage(game.Id);
            var response = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message);

            foreach (var pokemon in response.Cards.Select(id => game.FindCardById(id)).OfType<PokemonCard>())
            {
                pokemon.DamageCounters += Damage;
            }
        }
    }
}