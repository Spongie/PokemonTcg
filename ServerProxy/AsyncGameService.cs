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
public NetworkingCore.NetworkId AddToBench(NetworkingCore.NetworkId playerId,System.Collections.Generic.List`1[[TCGCards.PokemonCard, TCGCards, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]] pokemons)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "AddToBench",
			TargetClass = "GameService",
			Parameters = new object[] { playerId,pokemons }
		}.ToNetworkMessage(networkPlayer.Id);
		
		networkPlayer.Send(message);	
		return message.MessageId;
	} 
public NetworkingCore.NetworkId SetActivePokemon(NetworkingCore.NetworkId playerId,TCGCards.PokemonCard pokemon)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "SetActivePokemon",
			TargetClass = "GameService",
			Parameters = new object[] { playerId,pokemon }
		}.ToNetworkMessage(networkPlayer.Id);
		
		networkPlayer.Send(message);	
		return message.MessageId;
	} 
public NetworkingCore.NetworkId Attack(TCGCards.Attack attack)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "Attack",
			TargetClass = "GameService",
			Parameters = new object[] { attack }
		}.ToNetworkMessage(networkPlayer.Id);
		
		networkPlayer.Send(message);	
		return message.MessageId;
	} 
}