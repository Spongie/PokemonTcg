using NetworkingCore;
using NetworkingCore.Messages;

/// <summary>
/// Auto-generated code - DO NOT EDIT
/// </summary>
public class UserService
{
	private readonly NetworkPlayer networkPlayer;
	
	public UserService(NetworkPlayer networkPlayer)
	{
		this.networkPlayer = networkPlayer;
	}
	
	public NetworkingCore.BooleanResult Register(System.String userName,System.String password)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "Register",
			TargetClass = "UserService",
			Parameters = new object[] { userName,password }
		}.ToNetworkMessage(networkPlayer.Id);
	
		return networkPlayer.SendAndWaitForResponse<NetworkingCore.BooleanResult>(message);}public NetworkingCore.BooleanResult Login(System.String userName,System.String password)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "Login",
			TargetClass = "UserService",
			Parameters = new object[] { userName,password }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<NetworkingCore.BooleanResult>(message);
	}
}