using NetworkingCore;
using NetworkingCore.Messages;

/// <summary>
/// Auto-generated code - DO NOT EDIT
/// </summary>
public class AsyncGameService
{
	private readonly NetworkPlayer networkPlayer;
	
	public AsyncGameService(NetworkPlayer networkPlayer)
	{
		this.networkPlayer = networkPlayer;
	}
	
	public NetworkingCore.NetworkId HostGame(NetworkingCore.NetworkId hostPlayer)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "HostGame",
			TargetClass = "GameService",
			Parameters = new object[] { hostPlayer }
		}.ToNetworkMessage(networkPlayer.Id);
		
		networkPlayer.Send(message);	
		return message.MessageId;
	} 
public NetworkingCore.NetworkId JoinTheActiveGame(NetworkingCore.NetworkId playerToJoin)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "JoinTheActiveGame",
			TargetClass = "GameService",
			Parameters = new object[] { playerToJoin }
		}.ToNetworkMessage(networkPlayer.Id);
		
		networkPlayer.Send(message);	
		return message.MessageId;
	} 
public NetworkingCore.NetworkId AddToBench(NetworkingCore.NetworkId playerId,System.Collections.Generic.List<NetworkingCore.NetworkId> pokemonIds)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "AddToBench",
			TargetClass = "GameService",
			Parameters = new object[] { playerId,pokemonIds }
		}.ToNetworkMessage(networkPlayer.Id);
		
		networkPlayer.Send(message);	
		return message.MessageId;
	} 
public NetworkingCore.NetworkId EndTurn()
	{
		var message = new GenericMessageData
		{
			TargetMethod = "EndTurn",
			TargetClass = "GameService",
			Parameters = new object[] {  }
		}.ToNetworkMessage(networkPlayer.Id);
		
		networkPlayer.Send(message);	
		return message.MessageId;
	} 
public NetworkingCore.NetworkId SetActivePokemon(NetworkingCore.NetworkId playerId,NetworkingCore.NetworkId pokemonId)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "SetActivePokemon",
			TargetClass = "GameService",
			Parameters = new object[] { playerId,pokemonId }
		}.ToNetworkMessage(networkPlayer.Id);
		
		networkPlayer.Send(message);	
		return message.MessageId;
	} 
public NetworkingCore.NetworkId AttachEnergy(NetworkingCore.NetworkId targetId,NetworkingCore.NetworkId energyCardId)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "AttachEnergy",
			TargetClass = "GameService",
			Parameters = new object[] { targetId,energyCardId }
		}.ToNetworkMessage(networkPlayer.Id);
		
		networkPlayer.Send(message);	
		return message.MessageId;
	} 
public NetworkingCore.NetworkId ActivateAbility(NetworkingCore.NetworkId abilityId)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "ActivateAbility",
			TargetClass = "GameService",
			Parameters = new object[] { abilityId }
		}.ToNetworkMessage(networkPlayer.Id);
		
		networkPlayer.Send(message);	
		return message.MessageId;
	} 
public NetworkingCore.NetworkId Attack(NetworkingCore.NetworkId attackId)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "Attack",
			TargetClass = "GameService",
			Parameters = new object[] { attackId }
		}.ToNetworkMessage(networkPlayer.Id);
		
		networkPlayer.Send(message);	
		return message.MessageId;
	} 
public NetworkingCore.NetworkId PlayCard(NetworkingCore.NetworkId cardId)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "PlayCard",
			TargetClass = "GameService",
			Parameters = new object[] { cardId }
		}.ToNetworkMessage(networkPlayer.Id);
		
		networkPlayer.Send(message);	
		return message.MessageId;
	} 
public NetworkingCore.NetworkId EvolvePokemon(NetworkingCore.NetworkId basePokemonId,NetworkingCore.NetworkId targetPokemonId)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "EvolvePokemon",
			TargetClass = "GameService",
			Parameters = new object[] { basePokemonId,targetPokemonId }
		}.ToNetworkMessage(networkPlayer.Id);
		
		networkPlayer.Send(message);	
		return message.MessageId;
	} 
}