using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkingClient.Common
{
    public class NetworkPlayer
    {
        private TcpClient client;
        private NetworkStream stream;
        private Thread writingThread;
        private Thread readerThread;

        public event EventHandler<NetworkDataRecievedEventArgs> DataReceived;

        public NetworkPlayer(TcpClient tcpClient)
        {
            SetTcpClient(tcpClient);
        }

        private void SetTcpClient(TcpClient tcpClient)
        {
            client = tcpClient;

            stream = client.GetStream();

            readerThread = new Thread(ReadingThread);
            writingThread = new Thread(WritingThread);
            readerThread.Start();
            writingThread.Start();
        }

        void WritingThread()
        {

        }

        void ReadingThread()
        {
            while (true)
            {
                byte[] data;
                using(var inputStream = new MemoryStream())
                {
                    byte[] dataPrefix = new byte[sizeof(int)];
                    int receivedPrefixBytes = stream.Read(dataPrefix, 0, sizeof(int));

                    if(receivedPrefixBytes != 4)
                        continue;

                    int dataSize = BitConverter.ToInt32(dataPrefix, 0);
                    data = new byte[dataSize];

                    stream.Read(data, 0, data.Length);
                    inputStream.Write(data, 0, dataSize);

                    string input = Encoding.ASCII.GetString(inputStream.ToArray(), 0, (int)inputStream.Length);
                    var message = JsonConvert.DeserializeObject<NetworkMessage>(input);

                    DataReceived?.Invoke(this, new NetworkDataRecievedEventArgs(message));
                }
            }
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        
    }
}
