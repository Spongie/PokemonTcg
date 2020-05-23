using NetworkingCore;
using NetworkingCore.Messages;

/// <summary>
/// Auto-generated code - DO NOT EDIT
/// </summary>
public class PlayerService
{
	private readonly NetworkPlayer networkPlayer;
	
	public PlayerService(NetworkPlayer networkPlayer)
	{
		this.networkPlayer = networkPlayer;
	}
	
	public System.Int32 Login(System.String username,NetworkingCore.NetworkId playerId)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "Login",
			TargetClass = "PlayerService",
			Parameters = new object[] { username,playerId }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<System.Int32>(message);
	}
}