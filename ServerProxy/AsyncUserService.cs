using NetworkingCore;
using NetworkingCore.Messages;

/// <summary>
/// Auto-generated code - DO NOT EDIT
/// </summary>
public class AsyncUserService
{
	private readonly NetworkPlayer networkPlayer;
	
	public AsyncUserService(NetworkPlayer networkPlayer)
	{
		this.networkPlayer = networkPlayer;
	}
	
	public NetworkingCore.NetworkId Register(System.String userName,System.String password)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "Register",
			TargetClass = "UserService",
			Parameters = new object[] { userName,password }
		}.ToNetworkMessage(networkPlayer.Id);
		
		networkPlayer.Send(message);	
		return message.MessageId;
	} 
public NetworkingCore.NetworkId Login(System.String userName,System.String password)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "Login",
			TargetClass = "UserService",
			Parameters = new object[] { userName,password }
		}.ToNetworkMessage(networkPlayer.Id);
		
		networkPlayer.Send(message);	
		return message.MessageId;
	} 
}