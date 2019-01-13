using NetworkingCore;
using NetworkingCore.Messages;

/// <summary>
/// Auto-generated code - DO NOT EDIT
/// </summary>
public class AsyncImageService
{
	private readonly NetworkPlayer networkPlayer;
	
	public AsyncImageService(NetworkPlayer networkPlayer)
	{
		this.networkPlayer = networkPlayer;
	}
	
	public NetworkingCore.NetworkId LoadImage(System.String imageName)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "LoadImage",
			TargetClass = "ImageService",
			Parameters = new object[] { imageName }
		}.ToNetworkMessage(networkPlayer.Id);
		
		networkPlayer.Send(message);	
		return message.MessageId;
	} 
}