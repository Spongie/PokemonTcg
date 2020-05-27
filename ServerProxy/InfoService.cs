using NetworkingCore;
using NetworkingCore.Messages;

/// <summary>
/// Auto-generated code - DO NOT EDIT
/// </summary>
public class InfoService
{
	private readonly NetworkPlayer networkPlayer;
	
	public InfoService(NetworkPlayer networkPlayer)
	{
		this.networkPlayer = networkPlayer;
	}
	
	public TCGCards.Core.VersionNumber GetVersion()
	{
		var message = new GenericMessageData
		{
			TargetMethod = "GetVersion",
			TargetClass = "InfoService",
			Parameters = new object[] {  }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<TCGCards.Core.VersionNumber>(message);
	}
public System.Byte[] GetClientDownloadLink()
	{
		var message = new GenericMessageData
		{
			TargetMethod = "GetClientDownloadLink",
			TargetClass = "InfoService",
			Parameters = new object[] {  }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<System.Byte[]>(message);
	}
}