using NetworkingCore;
using NetworkingCore.Messages;

/// <summary>
/// Auto-generated code - DO NOT EDIT
/// </summary>
public class GameService
{
	private readonly NetworkPlayer networkPlayer;
	
	public GameService(NetworkPlayer networkPlayer)
	{
		this.networkPlayer = networkPlayer;
	}
	
	public System.Collections.Generic.List<TCGCards.Core.GameInfo> GetAvailableGames()
	{
		var message = new GenericMessageData
		{
			TargetMethod = "GetAvailableGames",
			TargetClass = "GameService",
			Parameters = new object[] {  }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<System.Collections.Generic.List<TCGCards.Core.GameInfo>>(message);
	}
public TCGCards.Core.GameField HostGame(NetworkingCore.NetworkId hostPlayer,System.Collections.Generic.List<System.Reflection.TypeInfo> deckInfo)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "HostGame",
			TargetClass = "GameService",
			Parameters = new object[] { hostPlayer,deckInfo }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<TCGCards.Core.GameField>(message);
	}
public TCGCards.Core.GameField JoinTheActiveGame(NetworkingCore.NetworkId playerToJoin,NetworkingCore.NetworkId gameToJoin,System.Collections.Generic.List<System.Reflection.TypeInfo> deckInfo)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "JoinTheActiveGame",
			TargetClass = "GameService",
			Parameters = new object[] { playerToJoin,gameToJoin,deckInfo }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<TCGCards.Core.GameField>(message);
	}
public TCGCards.Core.GameField AddToBench(NetworkingCore.NetworkId gameId,NetworkingCore.NetworkId playerId,System.Collections.Generic.List<NetworkingCore.NetworkId> pokemonIds)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "AddToBench",
			TargetClass = "GameService",
			Parameters = new object[] { gameId,playerId,pokemonIds }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<TCGCards.Core.GameField>(message);
	}
public TCGCards.Core.GameField EndTurn(NetworkingCore.NetworkId gameId)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "EndTurn",
			TargetClass = "GameService",
			Parameters = new object[] { gameId }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<TCGCards.Core.GameField>(message);
	}
public TCGCards.Core.GameField SetActivePokemon(NetworkingCore.NetworkId gameId,NetworkingCore.NetworkId playerId,NetworkingCore.NetworkId pokemonId)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "SetActivePokemon",
			TargetClass = "GameService",
			Parameters = new object[] { gameId,playerId,pokemonId }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<TCGCards.Core.GameField>(message);
	}
public TCGCards.Core.GameField AttachEnergy(NetworkingCore.NetworkId gameId,NetworkingCore.NetworkId targetId,NetworkingCore.NetworkId energyCardId)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "AttachEnergy",
			TargetClass = "GameService",
			Parameters = new object[] { gameId,targetId,energyCardId }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<TCGCards.Core.GameField>(message);
	}
public TCGCards.Core.GameField ActivateAbility(NetworkingCore.NetworkId gameId,NetworkingCore.NetworkId abilityId)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "ActivateAbility",
			TargetClass = "GameService",
			Parameters = new object[] { gameId,abilityId }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<TCGCards.Core.GameField>(message);
	}
public TCGCards.Core.GameField Attack(NetworkingCore.NetworkId gameId,NetworkingCore.NetworkId attackId)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "Attack",
			TargetClass = "GameService",
			Parameters = new object[] { gameId,attackId }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<TCGCards.Core.GameField>(message);
	}
public TCGCards.Core.GameField PlayCard(NetworkingCore.NetworkId gameId,NetworkingCore.NetworkId cardId)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "PlayCard",
			TargetClass = "GameService",
			Parameters = new object[] { gameId,cardId }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<TCGCards.Core.GameField>(message);
	}
public TCGCards.Core.GameField EvolvePokemon(NetworkingCore.NetworkId gameId,NetworkingCore.NetworkId basePokemonId,NetworkingCore.NetworkId targetPokemonId)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "EvolvePokemon",
			TargetClass = "GameService",
			Parameters = new object[] { gameId,basePokemonId,targetPokemonId }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<TCGCards.Core.GameField>(message);
	}
public TCGCards.Core.GameField RetreatPokemon(NetworkingCore.NetworkId gameId,NetworkingCore.NetworkId targetPokemon,System.Collections.Generic.List<NetworkingCore.NetworkId> energyCardIds)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "RetreatPokemon",
			TargetClass = "GameService",
			Parameters = new object[] { gameId,targetPokemon,energyCardIds }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<TCGCards.Core.GameField>(message);
	}
}