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
public TCGCards.Core.VersionNumber GetCardsVersion()
	{
		var message = new GenericMessageData
		{
			TargetMethod = "GetCardsVersion",
			TargetClass = "InfoService",
			Parameters = new object[] {  }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<TCGCards.Core.VersionNumber>(message);
	}
public System.Byte[] GetClientBytes()
	{
		var message = new GenericMessageData
		{
			TargetMethod = "GetClientBytes",
			TargetClass = "InfoService",
			Parameters = new object[] {  }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<System.Byte[]>(message);
	}
public System.String GetPokemonJson()
	{
		var message = new GenericMessageData
		{
			TargetMethod = "GetPokemonJson",
			TargetClass = "InfoService",
			Parameters = new object[] {  }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<System.String>(message);
	}
public System.String GetEnergyJson()
	{
		var message = new GenericMessageData
		{
			TargetMethod = "GetEnergyJson",
			TargetClass = "InfoService",
			Parameters = new object[] {  }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<System.String>(message);
	}
public System.String GetTrainerJson()
	{
		var message = new GenericMessageData
		{
			TargetMethod = "GetTrainerJson",
			TargetClass = "InfoService",
			Parameters = new object[] {  }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<System.String>(message);
	}
public System.String GetSetsJson()
	{
		var message = new GenericMessageData
		{
			TargetMethod = "GetSetsJson",
			TargetClass = "InfoService",
			Parameters = new object[] {  }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<System.String>(message);
	}
public System.String GetFormatsJson()
	{
		var message = new GenericMessageData
		{
			TargetMethod = "GetFormatsJson",
			TargetClass = "InfoService",
			Parameters = new object[] {  }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<System.String>(message);
	}
}