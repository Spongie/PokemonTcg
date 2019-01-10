using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NetworkingCore
{
    public class NetworkPlayer
    {
        public static bool logRequests = false;
        private TcpClient client;
        private NetworkStream stream;
        private Thread writingThread;
        private Thread readerThread;
        private ConcurrentQueue<NetworkMessage> messageQueue;
        public event EventHandler<NetworkDataRecievedEventArgs> DataReceived;

        public NetworkPlayer(TcpClient tcpClient)
        {
            messageQueue = new ConcurrentQueue<NetworkMessage>();
            SpecificResponses = new ConcurrentBag<NetworkMessage>();
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
            while(true)
            {
                if(!messageQueue.Any())
                {
                    Thread.Sleep(50);
                    continue;
                }

                if (messageQueue.TryDequeue(out NetworkMessage message))
                {
                    message.Send(stream);
                }
            }
        }

        void ReadingThread()
        {
            while(true)
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

                    if (message.MessageType == MessageTypes.Connected)
                    {
                        Id = (NetworkId)message.Data;
                    }
                    else if (message.ResponseTo.Value != Guid.Empty)
                    {
                        SpecificResponses.Add(message);
                    }
                    else
                    {
                        DataReceived?.Invoke(this, new NetworkDataRecievedEventArgs(message));
                    }
                }
            }
        }

        public T SendAndWaitForResponse<T>(NetworkMessage message)
        {
            Send(message);

            while (true)
            {
                Thread.Sleep(100);
                NetworkMessage response = SpecificResponses.FirstOrDefault(m => m.ResponseTo.Equals(message.MessageId));
                if (response != null)
                {
                    return (T)response.Data;
                }
            }
        }

        public NetworkId Id { get; set; }
        public string Name { get; set; }
        public ConcurrentBag<NetworkMessage> SpecificResponses { get; private set; }
    }
}
