using System;

namespace NetworkingClient.Common
{
    public class NetworkDataRecievedEventArgs : EventArgs
    {
        public NetworkDataRecievedEventArgs(NetworkMessage data)
        {
            Message = data;
        }

        public NetworkMessage Message { get; set; }
    }
}
