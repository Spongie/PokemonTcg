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
	
	public TCGCards.Core.GameField HostGame(NetworkingCore.NetworkId hostPlayer)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "HostGame",
			TargetClass = "GameService",
			Parameters = new object[] { hostPlayer }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<TCGCards.Core.GameField>(message);
	}
public TCGCards.Core.GameField JoinTheActiveGame(NetworkingCore.NetworkId playerToJoin)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "JoinTheActiveGame",
			TargetClass = "GameService",
			Parameters = new object[] { playerToJoin }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<TCGCards.Core.GameField>(message);
	}
public TCGCards.Core.GameField AddToBench(NetworkingCore.NetworkId playerId,System.Collections.Generic.List<NetworkingCore.NetworkId> pokemonIds)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "AddToBench",
			TargetClass = "GameService",
			Parameters = new object[] { playerId,pokemonIds }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<TCGCards.Core.GameField>(message);
	}
public TCGCards.Core.GameField EndTurn()
	{
		var message = new GenericMessageData
		{
			TargetMethod = "EndTurn",
			TargetClass = "GameService",
			Parameters = new object[] {  }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<TCGCards.Core.GameField>(message);
	}
public TCGCards.Core.GameField SetActivePokemon(NetworkingCore.NetworkId playerId,NetworkingCore.NetworkId pokemonId)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "SetActivePokemon",
			TargetClass = "GameService",
			Parameters = new object[] { playerId,pokemonId }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<TCGCards.Core.GameField>(message);
	}
public TCGCards.Core.GameField AttachEnergy(NetworkingCore.NetworkId targetId,NetworkingCore.NetworkId energyCardId)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "AttachEnergy",
			TargetClass = "GameService",
			Parameters = new object[] { targetId,energyCardId }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<TCGCards.Core.GameField>(message);
	}
public TCGCards.Core.GameField ActivateAbility(NetworkingCore.NetworkId abilityId)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "ActivateAbility",
			TargetClass = "GameService",
			Parameters = new object[] { abilityId }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<TCGCards.Core.GameField>(message);
	}
public TCGCards.Core.GameField Attack(NetworkingCore.NetworkId attackId)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "Attack",
			TargetClass = "GameService",
			Parameters = new object[] { attackId }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<TCGCards.Core.GameField>(message);
	}
public TCGCards.Core.GameField PlayCard(NetworkingCore.NetworkId cardId)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "PlayCard",
			TargetClass = "GameService",
			Parameters = new object[] { cardId }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<TCGCards.Core.GameField>(message);
	}
public TCGCards.Core.GameField EvolvePokemon(NetworkingCore.NetworkId basePokemonId,NetworkingCore.NetworkId targetPokemonId)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "EvolvePokemon",
			TargetClass = "GameService",
			Parameters = new object[] { basePokemonId,targetPokemonId }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<TCGCards.Core.GameField>(message);
	}
}