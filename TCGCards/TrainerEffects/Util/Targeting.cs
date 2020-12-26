using NetworkingCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.Messages;

namespace TCGCards.TrainerEffects.Util
{
    public static class Targeting
    {
        public static List<PokemonCard> GetPossibleTargetsFromMode(TargetingMode targetingMode, GameField game, Player caster, Player opponent, PokemonCard pokemonOwner)
        {
            switch (targetingMode)
            {
                case TargetingMode.YourActive:
                    return new List<PokemonCard> { caster.ActivePokemonCard };
                case TargetingMode.YourBench:
                    return caster.BenchedPokemon.ValidPokemonCards.ToList();
                case TargetingMode.YourPokemon:
                    return caster.GetAllPokemonCards();
                case TargetingMode.OpponentActive:
                    {
                        if (game.CurrentDefender != null)
                        {
                            return new List<PokemonCard> { game.CurrentDefender };
                        }

                        return new List<PokemonCard> { opponent.ActivePokemonCard };
                    }
                case TargetingMode.OpponentBench:
                    return opponent.BenchedPokemon.ValidPokemonCards.ToList();
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
                        return caster.BenchedPokemon.GetFirst();
                    }
                    message = new SelectFromYourBenchMessage(1) { Info = info }.ToNetworkMessage(game.Id);
                    selectedId = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.First();
                    target = (PokemonCard)game.Cards[selectedId];
                    break;
                case TargetingMode.YourPokemon:
                    message = new SelectFromYourPokemonMessage() { Info = info }.ToNetworkMessage(game.Id);
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

                    message = new SelectFromOpponentBenchMessage(1) { Info = info }.ToNetworkMessage(game.Id);
                    selectedId = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.First();
                    target = (PokemonCard)game.Cards[selectedId];
                    break;
                case TargetingMode.OpponentPokemon:
                    message = new SelectOpponentPokemonMessage(1) { Info = info }.ToNetworkMessage(game.Id);
                    selectedId = caster.NetworkPlayer.SendAndWaitForResponse<CardListMessage>(message).Cards.First();
                    target = (PokemonCard)game.Cards[selectedId];
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
