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
                case CardType.BasicPokemon:
                    return cards.OfType<PokemonCard>().Where(pokemon => pokemon.Stage == 0).OfType<Card>().ToList();
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
                case CardType.BasicPokemon:
                    filter.Add(new BasicPokemonFilter());
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

        public static List<PokemonCard> GetPossibleTargetsFromMode(TargetingMode targetingMode, GameField game, Player caster, Player opponent, PokemonCard pokemonOwner)
        {
            switch (targetingMode)
            {
                case TargetingMode.YourActive:
                    return new List<PokemonCard> { caster.ActivePokemonCard };
                case TargetingMode.YourBench:
                    return caster.BenchedPokemon;
                case TargetingMode.YourPokemon:
                    return caster.GetAllPokemonCards();
                case TargetingMode.OpponentActive:
                    return new List<PokemonCard> { opponent.ActivePokemonCard };
                case TargetingMode.OpponentBench:
                    return opponent.BenchedPokemon;
                case TargetingMode.OpponentPokemon:
                    return opponent.GetAllPokemonCards();
                case TargetingMode.AnyPokemon:
                    var pokemons = caster.GetAllPokemonCards();
                    pokemons.AddRange(opponent.GetAllPokemonCards());
                    return pokemons;
                case TargetingMode.AttachedTo:
                    return new List<PokemonCard> { pokemonOwner };
                case TargetingMode.Self:
                    return new List<PokemonCard> { pokemonOwner };
                default:
                    return new List<PokemonCard> { pokemonOwner };
            }
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
                    if (caster.BenchedPokemon.Count == 0)
                    {
                        return null;
                    }
                    else if (caster.BenchedPokemon.Count == 1)
                    {
                        return caster.BenchedPokemon[0];
                    }
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
                    if (opponent.BenchedPokemon.Count == 0)
                    {
                        return null;
                    }
                    else if (opponent.BenchedPokemon.Count == 1)
                    {
                        return opponent.BenchedPokemon[0];
                    }

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
