using NetworkingCore;
using NetworkingCore.Messages;
using Proxy;

[MakeSyncProxy(typeof(ICardService))]
public partial class CardService
{
    private readonly NetworkPlayer networkPlayer;

    public CardService(NetworkPlayer networkPlayer)
    {
        this.networkPlayer = networkPlayer;
    }
}