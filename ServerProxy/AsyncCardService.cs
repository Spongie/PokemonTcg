using NetworkingCore;
using NetworkingCore.Messages;
using Proxy;

[MakeAsyncProxy(typeof(ICardService))]
public partial class AsyncCardService
{
    private readonly NetworkPlayer networkPlayer;

    public AsyncCardService(NetworkPlayer networkPlayer)
    {
        this.networkPlayer = networkPlayer;
    }
}