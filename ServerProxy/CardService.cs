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
	
	public System.Collections.Generic.List<TCGCards.Card> GetAllCards()
	{
		var message = new GenericMessageData
		{
			TargetMethod = "GetAllCards",
			TargetClass = "CardService",
			Parameters = new object[] {  }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<System.Collections.Generic.List<TCGCards.Card>>(message);
	}
public System.Collections.Generic.List<Entities.Models.Set> GetAllSets()
	{
		var message = new GenericMessageData
		{
			TargetMethod = "GetAllSets",
			TargetClass = "CardService",
			Parameters = new object[] {  }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<System.Collections.Generic.List<Entities.Models.Set>>(message);
	}
public System.Boolean UpdateCards(System.String pokemonCards,System.String energyCards,System.String tainerCards,System.String sets)
	{
		var message = new GenericMessageData
		{
			TargetMethod = "UpdateCards",
			TargetClass = "CardService",
			Parameters = new object[] { pokemonCards,energyCards,tainerCards,sets }
		}.ToNetworkMessage(networkPlayer.Id);
		
		return networkPlayer.SendAndWaitForResponse<System.Boolean>(message);
	}
}