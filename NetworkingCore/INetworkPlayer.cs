namespace NetworkingCore
{
    public interface INetworkPlayer
    {
        void Send(NetworkMessage networkMessage);
        T SendAndWaitForResponse<T>(NetworkMessage message);

        NetworkId Id { get; set; }
        string Name { get; set; }
    }
}
