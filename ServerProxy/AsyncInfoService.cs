using NetworkingCore;
using NetworkingCore.Messages;

/// <summary>
/// Auto-generated code - DO NOT EDIT
/// </summary>
public class AsyncInfoService
{
	private readonly NetworkPlayer networkPlayer;
	
	public AsyncInfoService(NetworkPlayer networkPlayer)
	{
		this.networkPlayer = networkPlayer;
	}
	
	public NetworkingCore.NetworkId GetVersion()
	{
		var message = new GenericMessageData
		{
			TargetMethod = "GetVersion",
			TargetClass = "InfoService",
			Parameters = new object[] {  }
		}.ToNetworkMessage(networkPlayer.Id);
		
		networkPlayer.Send(message);	
		return message.MessageId;
	} 
public NetworkingCore.NetworkId GetClientDownloadLink()
	{
		var message = new GenericMessageData
		{
			TargetMethod = "GetClientDownloadLink",
			TargetClass = "InfoService",
			Parameters = new object[] {  }
		}.ToNetworkMessage(networkPlayer.Id);
		
		networkPlayer.Send(message);	
		return message.MessageId;
	} 
}