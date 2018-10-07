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
	
	public System.Guid GetCardSets()
	{
		var message = new GenericMessageData
		{
			TargetMethod = "GetCardSets",
			TargetClass = "CardService",
			Parameters = new object[] {  }
		}.ToNetworkMessage(networkPlayer.Id);
		
		networkPlayer.Send(message);	
		return message.MessageId;
	}
public System.Guid GetFormats()
	{
		var message = new GenericMessageData
		{
			TargetMethod = "GetFormats",
			TargetClass = "CardService",
			Parameters = new object[] {  }
		}.ToNetworkMessage(networkPlayer.Id);
		
		networkPlayer.Send(message);	
		return message.MessageId;
	}
}