using Entities;
using NetworkingCore;
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
        public static List<Card> GetCardsOfType(List<Card> cards, CardType cardType)
        {
            switch (cardType)
            {
                case CardType.Pokemon:
                    return cards.OfType<PokemonCard>().OfType<Card>().ToList();
                case CardType.Trainer:
                    return cards.OfType<TrainerCard>().OfType<Card>().ToList();
                case CardType.Energy:
                    return cards.OfType<EnergyCard>().OfType<Card>().ToList();
                case CardType.BasicEnergy:
                    return cards.OfType<EnergyCard>().Where(energy => energy.IsBasic).OfType<Card>().ToList();
                case CardType.Any:
                default:
                    return cards;
            }
        }

        public static List<IDeckFilter> GetCardFilters(CardType cardType, EnergyTypes energyType = EnergyTypes.None)
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
                    if (energyType == EnergyTypes.All || energyType == EnergyTypes.None)
                    {
                        filter.Add(new BasicEnergyFilter());
                    }
                    else
                    {
                        filter.Add(new EnergyTypeFilter(energyType));
                    }
                    break;
                default:
                    break;
            }

            return filter;
        }

        public static PokemonCard AskForTargetFromTargetingMode(TargetingMode targetingMode, GameField game, Player caster, Player opponent, PokemonCard pokemonOwner, string info = "")
        {
            PokemonCard target;
            NetworkMessage message;
            NetworkId selectedId;

            switch (targetingMode)
            {
                case TargetingMode.Self:
                    target = pokemonOwner;
                    break;
                case TargetingMode.YourActive:
                    target = caster.ActivePokemonCard;
                    break;
                case TargetingMode.YourBench:
                    message = new SelectFromYourBenchMessage(1) { Info = info }.ToNetworkMessage(game.Id);
                    selectedId = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.First();
                    target = (PokemonCard)game.FindCardById(selectedId);
                    break;
                case TargetingMode.YourPokemon:
                    message = new SelectFromYourPokemonMessage() { Info = info }.ToNetworkMessage(game.Id);
                    selectedId = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.First();
                    target = (PokemonCard)game.FindCardById(selectedId);
                    break;
                case TargetingMode.OpponentActive:
                    target = opponent.ActivePokemonCard;
                    break;
                case TargetingMode.OpponentBench:
                    message = new SelectFromOpponentBenchMessage(1) { Info = info }.ToNetworkMessage(game.Id);
                    selectedId = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.First();
                    target = (PokemonCard)game.FindCardById(selectedId);
                    break;
                case TargetingMode.OpponentPokemon:
                    message = new SelectOpponentPokemonMessage(1) { Info = info }.ToNetworkMessage(game.Id);
                    selectedId = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.First();
                    target = (PokemonCard)game.FindCardById(selectedId);
                    break;
                case TargetingMode.AnyPokemon:
                    throw new NotImplementedException("TargetingMode.AnyPokemon not implemented in CardUtil");
                default:
                    target = caster.ActivePokemonCard;
                    break;
            }

            return target;
        }
    }
}
