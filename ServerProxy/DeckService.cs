using NetworkingCore;
using NetworkingCore.Messages;

/// <summary>
/// Auto-generated code - DO NOT EDIT
/// </summary>
public class DeckService
{
	private readonly NetworkPlayer networkPlayer;
	
	public DeckService(NetworkPlayer networkPlayer)
	{
		this.networkPlayer = networkPlayer;
	}
	
	public NetworkingCore.LongResult SaveDeck(Entities.DeckInfo deck)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "SaveDeck",
			TargetClass = "DeckService",
			Parameters = new object[] { deck }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<NetworkingCore.LongResult>(message);
	}
}