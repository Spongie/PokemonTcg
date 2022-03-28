using NetworkingCore;
using NetworkingCore.Messages;
using Proxy;

[MakeAsyncProxy(typeof(IGameService))]
public partial class AsyncGameService
{
    private readonly NetworkPlayer networkPlayer;

    public AsyncGameService(NetworkPlayer networkPlayer)
    {
        this.networkPlayer = networkPlayer;
    }
}