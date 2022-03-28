using NetworkingCore;
using NetworkingCore.Messages;
using Proxy;

[MakeSyncProxy(typeof(IGameService))]
public partial class GameService
{
    private readonly NetworkPlayer networkPlayer;

    public GameService(NetworkPlayer networkPlayer)
    {
        this.networkPlayer = networkPlayer;
    }
}