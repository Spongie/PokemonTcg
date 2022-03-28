using NetworkingCore;
using NetworkingCore.Messages;
using Proxy;

[MakeAsyncProxy(typeof(IInfoService))]
public partial class AsyncInfoService
{
    private readonly NetworkPlayer networkPlayer;

    public AsyncInfoService(NetworkPlayer networkPlayer)
    {
        this.networkPlayer = networkPlayer;
    }
}