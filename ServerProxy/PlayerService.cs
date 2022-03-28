using NetworkingCore;
using NetworkingCore.Messages;
using Proxy;

[MakeSyncProxy(typeof(IPlayerService))]
public partial class PlayerService
{
    private readonly NetworkPlayer networkPlayer;

    public PlayerService(NetworkPlayer networkPlayer)
    {
        this.networkPlayer = networkPlayer;
    }
}
