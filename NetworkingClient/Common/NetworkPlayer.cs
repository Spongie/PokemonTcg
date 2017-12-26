using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TCGCards.Core;

namespace NetworkingClient.Common
{
    public class NetworkPlayer
    {
        private TcpClient client;
        private NetworkStream stream;
        private Thread writingThread;
        private Thread readerThread;
        private Queue<NetworkMessage> messageQueue;
        public event EventHandler<NetworkDataRecievedEventArgs> DataReceived;
        
        public NetworkPlayer(TcpClient tcpClient)
        {
            messageQueue = new Queue<NetworkMessage>();
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

        public void Send(NetworkMessage networkMessage)
        {
            messageQueue.Enqueue(networkMessage);
        }

        void WritingThread()
        {
            while (true)
            {
                if (!messageQueue.Any())
                {
                    Thread.Sleep(50);
                    continue;
                }

                var message = messageQueue.Dequeue();
                message.Send(stream);
            }
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
                    {
                        Thread.Sleep(50);
                        continue;
                    }

                    int dataSize = BitConverter.ToInt32(dataPrefix, 0);
                    data = new byte[dataSize];

                    stream.Read(data, 0, data.Length);
                    inputStream.Write(data, 0, dataSize);

                    string input = Encoding.UTF8.GetString(inputStream.ToArray(), 0, (int)inputStream.Length);
                    var message = Serializer.Deserialize<NetworkMessage>(input);

                    DataReceived?.Invoke(this, new NetworkDataRecievedEventArgs(message));
                }
            }
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Player Player { get; set; }
    }
}
