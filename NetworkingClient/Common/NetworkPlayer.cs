using NetworkingCore;
using System.Net.Sockets;
using TCGCards.Core;

namespace NetworkingClient.Common
{
    public class ServerPlayer : NetworkPlayer
    {
        public ServerPlayer(TcpClient tcpClient) : base(tcpClient)
        {
        }

        public Player Player { get; set; }
    }
}
