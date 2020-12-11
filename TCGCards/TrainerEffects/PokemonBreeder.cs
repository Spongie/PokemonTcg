using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCGCards.Core;
using TCGCards.Core.Deckfilters;
using TCGCards.Core.GameEvents;
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
            throw new NotImplementedException();
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            var responseStage2 = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(new DiscardCardsMessage(1, new List<IDeckFilter> { new Stage2Filter() }).ToNetworkMessage(game.Id));
            var evolutionCard = (PokemonCard)game.FindCardById(responseStage2.Cards.First());
            var basicVersionName = PokemonNames.GetBasicVersionOf(evolutionCard.PokemonName);

            var availableCards = new List<PokemonCard>();

            foreach (var pokemon in caster.GetAllPokemonCards().Where(p => p.PokemonName == basicVersionName))
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

            var selectedCard = (PokemonCard)game.FindCardById(pickTargetResponse.Cards.First());

            game.EvolvePokemon(selectedCard, evolutionCard, true);
        }
    }
}
