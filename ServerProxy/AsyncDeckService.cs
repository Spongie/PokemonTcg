using NetworkingCore;
using NetworkingCore.Messages;

/// <summary>
/// Auto-generated code - DO NOT EDIT
/// </summary>
public class AsyncDeckService
{
	private readonly NetworkPlayer networkPlayer;
	
	public AsyncDeckService(NetworkPlayer networkPlayer)
	{
		this.networkPlayer = networkPlayer;
	}
	
	public NetworkingCore.NetworkId SaveDeck(Entities.DeckInfo deck)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "SaveDeck",
			TargetClass = "DeckService",
			Parameters = new object[] { deck }
		}.ToNetworkMessage(networkPlayer.Id);
		
		networkPlayer.Send(message);	
		return message.MessageId;
	} 
}