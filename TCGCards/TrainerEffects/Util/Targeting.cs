using Entities;
using NetworkingCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Deckfilters;
using TCGCards.Core.Messages;

namespace TCGCards.TrainerEffects.Util
{
    public static class Targeting
    {
        public static List<PokemonCard> GetPossibleTargetsFromMode(TargetingMode targetingMode, GameField game, Player caster, Player opponent, PokemonCard pokemonOwner, string nameFilter = "")
        {
            var pokemons = new List<PokemonCard>();
            switch (targetingMode)
            {
                case TargetingMode.YourActive:
                    pokemons.Add(caster.ActivePokemonCard);
                    break;
                case TargetingMode.YourBench:
                    pokemons = caster.BenchedPokemon.ValidPokemonCards.ToList();
                    break;
                case TargetingMode.YourPokemon:
                    pokemons = caster.GetAllPokemonCards();
                    break;
                case TargetingMode.OpponentActive:
                    {
                        if (game.CurrentDefender != null)
                        {
                            pokemons.Add(game.CurrentDefender);
                        }
                        else
                        {
                            pokemons.Add(opponent.ActivePokemonCard);
                        }
                        break;
                    }
                case TargetingMode.OpponentBench:
                    pokemons = opponent.BenchedPokemon.ValidPokemonCards.ToList();
                    break;
                case TargetingMode.OpponentPokemon:
                    pokemons = opponent.GetAllPokemonCards();
                    break;
                case TargetingMode.AnyPokemon:
                    pokemons = caster.GetAllPokemonCards();
                    pokemons.AddRange(opponent.GetAllPokemonCards());
                    break;
                case TargetingMode.AttachedTo:
                    pokemons.Add(pokemonOwner);
                    break;
                case TargetingMode.Self:
                    pokemons.Add(pokemonOwner);
                    break;
                default:
                    pokemons.Add(pokemonOwner);
                    break;
            }

            if (!string.IsNullOrEmpty(nameFilter))
            {
                return pokemons.Where(p => p.Name.ToLower().Contains(nameFilter.ToLower())).ToList();
            }

            return pokemons;
        }

        public static PokemonCard AskForTargetFromTargetingMode(TargetingMode targetingMode, GameField game, Player caster, Player opponent, PokemonCard pokemonOwner, string info = "", string nameFilter = "")
        {
            PokemonCard target;
            NetworkMessage message;
            NetworkId selectedId;
            IDeckFilter filter = !string.IsNullOrEmpty(nameFilter) ? new PokemonWithNameOrTypeFilter(nameFilter, EnergyTypes.All) : null;

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
                        return caster.BenchedPokemon.GetFirst();
                    }
                    message = new SelectFromYourBenchMessage(1) { Info = info, Filter = filter }.ToNetworkMessage(game.Id);
                    selectedId = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.First();
                    target = (PokemonCard)game.Cards[selectedId];
                    break;
                case TargetingMode.YourPokemon:
                    message = new SelectFromYourPokemonMessage() { Info = info, Filter = filter }.ToNetworkMessage(game.Id);
                    selectedId = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.First();
                    target = (PokemonCard)game.Cards[selectedId];
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
                        return opponent.BenchedPokemon.GetFirst();
                    }

                    message = new SelectFromOpponentBenchMessage(1) { Info = info, Filter = filter }.ToNetworkMessage(game.Id);
                    selectedId = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.First();
                    target = (PokemonCard)game.Cards[selectedId];
                    break;
                case TargetingMode.OpponentPokemon:
                    message = new SelectOpponentPokemonMessage(1) { Info = info, Filter = filter }.ToNetworkMessage(game.Id);
                    selectedId = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.First();
                    target = (PokemonCard)game.Cards[selectedId];
                    break;
                case TargetingMode.AnyPokemon:
                    throw new NotImplementedException("TargetingMode.AnyPokemon not implemented in Targeting");
                default:
                    target = caster.ActivePokemonCard;
                    break;
            }

            if (game != null)
            {
                game.LastTarget = target;
            }

            return target;
        }
    }
}
