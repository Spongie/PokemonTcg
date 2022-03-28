using NetworkingCore;
using NetworkingCore.Messages;
using Proxy;

[MakeAsyncProxy(typeof(IImageService))]
public partial class AsyncImageService
{
    private readonly NetworkPlayer networkPlayer;

    public AsyncImageService(NetworkPlayer networkPlayer)
    {
        this.networkPlayer = networkPlayer;
    }
}