using NetworkingCore;
using NetworkingCore.Messages;

/// <summary>
/// Auto-generated code - DO NOT EDIT
/// </summary>
public class AsyncPlayerService
{
	private readonly NetworkPlayer networkPlayer;
	
	public AsyncPlayerService(NetworkPlayer networkPlayer)
	{
		this.networkPlayer = networkPlayer;
	}
	
	public NetworkingCore.NetworkId Login(System.String username,NetworkingCore.NetworkId playerId)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "Login",
			TargetClass = "PlayerService",
			Parameters = new object[] { username,playerId }
		}.ToNetworkMessage(networkPlayer.Id);
		
		networkPlayer.Send(message);	
		return message.MessageId;
	} 
}