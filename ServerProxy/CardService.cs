using NetworkingCore;
using NetworkingCore.Messages;

/// <summary>
/// Auto-generated code - DO NOT EDIT
/// </summary>
public class CardService
{
	private readonly NetworkPlayer networkPlayer;
	
	public CardService(NetworkPlayer networkPlayer)
	{
		this.networkPlayer = networkPlayer;
	}
	
	public System.Collections.Generic.List<Entities.CardSet> GetCardSets()
	{
		var message = new GenericMessageData
		{
			TargetMethod = "GetCardSets",
			TargetClass = "CardService",
			Parameters = new object[] {  }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<System.Collections.Generic.List<Entities.CardSet>>(message);
	}
public System.Collections.Generic.List<Entities.Format> GetFormats()
	{
		var message = new GenericMessageData
		{
			TargetMethod = "GetFormats",
			TargetClass = "CardService",
			Parameters = new object[] {  }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<System.Collections.Generic.List<Entities.Format>>(message);
	}
}