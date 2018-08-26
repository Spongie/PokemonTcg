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
	
	public System.Boolean Register(System.String userName,System.String password)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "Register",
			TargetClass = "UserService",
			Parameters = new object[] { userName,password }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<System.Boolean>(message);
	}
public System.Boolean Login(System.String userName,System.String password)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "Login",
			TargetClass = "UserService",
			Parameters = new object[] { userName,password }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<System.Boolean>(message);
	}
}