using NetworkingCore;
using NetworkingCore.Messages;

/// <summary>
/// Auto-generated code - DO NOT EDIT
/// </summary>
public class ImageService
{
	private readonly NetworkPlayer networkPlayer;
	
	public ImageService(NetworkPlayer networkPlayer)
	{
		this.networkPlayer = networkPlayer;
	}
	
	public System.String LoadImage(System.String imageName)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "LoadImage",
			TargetClass = "ImageService",
			Parameters = new object[] { imageName }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<System.String>(message);
	}
}