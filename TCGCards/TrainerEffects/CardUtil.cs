﻿using NetworkingCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Deckfilters;
using TCGCards.Core.Messages;

namespace TCGCards.TrainerEffects
{
    public static class CardUtil
    {
        public static IEnumerable<Card> GetCardsOfType(IEnumerable<Card> cards, CardType cardType)
        {
            switch (cardType)
            {
                case CardType.Pokemon:
                    return cards.OfType<PokemonCard>();
                case CardType.Trainer:
                    return cards.OfType<TrainerCard>();
                case CardType.Energy:
                    return cards.OfType<EnergyCard>();
                case CardType.BasicEnergy:
                    return cards.OfType<EnergyCard>().Where(energy => energy.IsBasic);
                case CardType.Any:
                default:
                    return cards;
            }
        }

        public static IEnumerable<IDeckFilter> GetCardFilters(CardType cardType)
        {
            var filter = new List<IDeckFilter>();

            switch (cardType)
            {
                case CardType.Pokemon:
                    filter.Add(new PokemonFilter());
                    break;
                case CardType.Trainer:
                    filter.Add(new TrainerFilter());
                    break;
                case CardType.Energy:
                    filter.Add(new EnergyFilter());
                    break;
                case CardType.BasicEnergy:
                    filter.Add(new BasicEnergyFilter());
                    break;
                default:
                    break;
            }

            return filter;
        }

        public static PokemonCard AskForTargetFromTargetingMode(TargetingMode targetingMode, GameField game, Player caster, Player opponent)
        {
            PokemonCard target;
            NetworkMessage message;
            NetworkId selectedId;

            switch (targetingMode)
            {
                case TargetingMode.YourActive:
                    target = caster.ActivePokemonCard;
                    break;
                case TargetingMode.YourBench:
                    message = new SelectFromYourBenchMessage(1).ToNetworkMessage(game.Id);
                    selectedId = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.First();
                    target = (PokemonCard)game.FindCardById(selectedId);
                    break;
                case TargetingMode.YourPokemon:
                    message = new SelectFromYourPokemonMessage().ToNetworkMessage(game.Id);
                    selectedId = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.First();
                    target = (PokemonCard)game.FindCardById(selectedId);
                    break;
                case TargetingMode.OpponentActive:
                    target = opponent.ActivePokemonCard;
                    break;
                case TargetingMode.OpponentBench:
                    message = new SelectFromOpponentBenchMessage(1).ToNetworkMessage(game.Id);
                    selectedId = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.First();
                    target = (PokemonCard)game.FindCardById(selectedId);
                    break;
                case TargetingMode.OpponentPokemon:
                    message = new SelectFromOpponentBenchMessage(1).ToNetworkMessage(game.Id);
                    selectedId = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.First();
                    target = (PokemonCard)game.FindCardById(selectedId);
                    break;
                case TargetingMode.AnyPokemon:
                    throw new NotImplementedException("TargetingMode.AnyPokemon not implemented in fullheal");
                default:
                    target = caster.ActivePokemonCard;
                    break;
            }

            return target;
        }
    }
}
