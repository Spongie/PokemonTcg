using NetworkingCore;
using System.Collections.Generic;
using TCGCards.Core;

public interface IGameService
{
    GameField ActivateAbility(NetworkId gameId, NetworkId abilityId);
    GameField AddToBench(NetworkId gameId, NetworkId playerId, List<NetworkId> pokemonIds);
    GameField AttachEnergy(NetworkId gameId, NetworkId targetId, NetworkId energyCardId);
    GameField Attack(NetworkId gameId, NetworkId attackId);
    GameField EndTurn(NetworkId gameId);
    GameField EvolvePokemon(NetworkId gameId, NetworkId basePokemonId, NetworkId targetPokemonId);
    List<GameInfo> GetAvailableGames();
    GameField GetGameField(NetworkId gameId);
    GameField HostGame(NetworkId hostPlayer, Deck deckInfo);
    GameField JoinTheActiveGame(NetworkId playerToJoin, NetworkId gameToJoin, Deck deckInfo);
    GameField LeaveGame(NetworkId playerId, NetworkId gameId);
    GameField PlayCard(NetworkId gameId, NetworkId cardId);
    GameField RetreatPokemon(NetworkId gameId, NetworkId targetPokemon, List<NetworkId> energyCardIds);
    GameField SetActivePokemon(NetworkId gameId, NetworkId playerId, NetworkId pokemonId);
}