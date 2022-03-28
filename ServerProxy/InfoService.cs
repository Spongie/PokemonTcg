using NetworkingCore;
using NetworkingCore.Messages;
using Proxy;

[MakeSyncProxy(typeof(IInfoService))]
public partial class InfoService
{
    private readonly NetworkPlayer networkPlayer;

    public InfoService(NetworkPlayer networkPlayer)
    {
        this.networkPlayer = networkPlayer;
    }
}