using NetworkingCore;
using Proxy;

[MakeSyncProxy(typeof(IImageService))]
public partial class ImageService
{
    private readonly NetworkPlayer networkPlayer;

    public ImageService(NetworkPlayer networkPlayer)
    {
        this.networkPlayer = networkPlayer;
    }
}
