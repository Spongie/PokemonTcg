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
public NetworkingCore.NetworkId AddToBench(NetworkingCore.NetworkId playerId,System.Collections.Generic.List<TCGCards.PokemonCard> pokemons)
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
public NetworkingCore.NetworkId AttachEnergy(TCGCards.PokemonCard target,EnergyCard energyCard)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "AttachEnergy",
			TargetClass = "GameService",
			Parameters = new object[] { target,energyCard }
		}.ToNetworkMessage(networkPlayer.Id);
		
		networkPlayer.Send(message);	
		return message.MessageId;
	} 
public NetworkingCore.NetworkId ActivateAbility(TCGCards.Core.Ability ability)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "ActivateAbility",
			TargetClass = "GameService",
			Parameters = new object[] { ability }
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
public NetworkingCore.NetworkId PlayCard(TCGCards.Card card)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "PlayCard",
			TargetClass = "GameService",
			Parameters = new object[] { card }
		}.ToNetworkMessage(networkPlayer.Id);
		
		networkPlayer.Send(message);	
		return message.MessageId;
	} 
public NetworkingCore.NetworkId EvolvePokemon(TCGCards.PokemonCard card,TCGCards.PokemonCard target)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "EvolvePokemon",
			TargetClass = "GameService",
			Parameters = new object[] { card,target }
		}.ToNetworkMessage(networkPlayer.Id);
		
		networkPlayer.Send(message);	
		return message.MessageId;
	} 
}