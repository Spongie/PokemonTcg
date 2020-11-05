using NetworkingCore;
using NetworkingCore.Messages;

/// <summary>
/// Auto-generated code - DO NOT EDIT
/// </summary>
public class AsyncCardService
{
	private readonly NetworkPlayer networkPlayer;
	
	public AsyncCardService(NetworkPlayer networkPlayer)
	{
		this.networkPlayer = networkPlayer;
	}
	
	public NetworkingCore.NetworkId GetAllCards()
	{
		var message = new GenericMessageData
		{
			TargetMethod = "GetAllCards",
			TargetClass = "CardService",
			Parameters = new object[] {  }
		}.ToNetworkMessage(networkPlayer.Id);
		
		networkPlayer.Send(message);	
		return message.MessageId;
	} 
public NetworkingCore.NetworkId GetAllSets()
	{
		var message = new GenericMessageData
		{
			TargetMethod = "GetAllSets",
			TargetClass = "CardService",
			Parameters = new object[] {  }
		}.ToNetworkMessage(networkPlayer.Id);
		
		networkPlayer.Send(message);	
		return message.MessageId;
	} 
public NetworkingCore.NetworkId UpdateCards(System.String pokemonCards,System.String energyCards,System.String tainerCards,System.String sets)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "UpdateCards",
			TargetClass = "CardService",
			Parameters = new object[] { pokemonCards,energyCards,tainerCards,sets }
		}.ToNetworkMessage(networkPlayer.Id);
		
		networkPlayer.Send(message);	
		return message.MessageId;
	} 
}