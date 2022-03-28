using NetworkingCore;
using NetworkingCore.Messages;
using Proxy;

[MakeAsyncProxy(typeof(IPlayerService))]
public partial class AsyncPlayerService
{
    private readonly NetworkPlayer networkPlayer;

    public AsyncPlayerService(NetworkPlayer networkPlayer)
    {
        this.networkPlayer = networkPlayer;
    }
}