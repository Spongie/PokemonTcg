﻿using System.Collections.Generic;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Deckfilters;
using TCGCards.Core.Messages;

namespace TCGCards.TrainerEffects
{
    public class PokemonBreeder : IEffect
    {
        public string EffectType
        {
            get
            {
                return "Pokémon Breeder";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            foreach (var pokemon in caster.Hand.OfType<PokemonCard>())
            {
                if (pokemon.Stage < 2)
                {
                    continue;
                }

                var basicName = PokemonNames.GetBasicVersionOf(pokemon.PokemonName);

                foreach (var basicPokemon in caster.GetAllPokemonCards().Where(x => x.PokemonName == basicName))
                {
                    if (basicPokemon.CanEvolve())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            var discardMessage = new DiscardCardsMessage(1, new List<IDeckFilter> { new Stage2Filter() })
            {
                Info = "Select a stage 2 Pokémon from your hand"
            };

            var responseStage2 = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(discardMessage.ToNetworkMessage(game.Id));
            var evolutionCard = (PokemonCard)game.Cards[responseStage2.Cards.First()];
            var basicVersionName = PokemonNames.GetBasicVersionOf(evolutionCard.PokemonName);

            var availableCards = new List<PokemonCard>();

            foreach (var pokemon in caster.GetAllPokemonCards().Where(p => p.PokemonName == basicVersionName && p.CanEvolve()))
            {
                availableCards.Add(pokemon);
            }

            if (availableCards.Count == 1)
            {
                game.EvolvePokemon(availableCards[0], evolutionCard, true);

                return;
            }

            var pickTargetResponse = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new SelectFromYourPokemonMessage()
            {
                Info = $"Select a {basicVersionName} Pokémon to evolve",
                Filter = new PokemonWithNameAndOwner() { Owner = caster.Id,  Names = availableCards.Select(x => x.PokemonName).Distinct().ToList() }
            }.ToNetworkMessage(game.Id));

            var selectedCard = (PokemonCard)game.Cards[pickTargetResponse.Cards.First()];

            game.EvolvePokemon(selectedCard, evolutionCard, true);
        }
    }
}
