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
public TCGCards.Core.GameField AddToBench(NetworkingCore.NetworkId playerId,System.Collections.Generic.List<TCGCards.PokemonCard> pokemons)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "AddToBench",
			TargetClass = "GameService",
			Parameters = new object[] { playerId,pokemons }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<TCGCards.Core.GameField>(message);
	}
public TCGCards.Core.GameField SetActivePokemon(NetworkingCore.NetworkId playerId,TCGCards.PokemonCard pokemon)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "SetActivePokemon",
			TargetClass = "GameService",
			Parameters = new object[] { playerId,pokemon }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<TCGCards.Core.GameField>(message);
	}
public TCGCards.Core.GameField Attack(TCGCards.Attack attack)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "Attack",
			TargetClass = "GameService",
			Parameters = new object[] { attack }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<TCGCards.Core.GameField>(message);
	}
}