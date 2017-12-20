using System;

namespace NetworkingClient.Common
{
    public class NetworkDataRecievedEventArgs : EventArgs
    {
        public NetworkDataRecievedEventArgs(string data)
        {
            Data = data;
        }

        public string Data { get; set; }
    }
}
