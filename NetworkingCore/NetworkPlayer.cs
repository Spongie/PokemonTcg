using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NetworkingCore
{
    public class NetworkPlayer : INetworkPlayer
    {
        private TcpClient client;
        private NetworkStream stream;
        private Thread writingThread;
        private Thread readerThread;
        private ConcurrentQueue<NetworkMessage> messageQueue;
        public event EventHandler<NetworkDataRecievedEventArgs> DataReceived;
        public event EventHandler<NetworkId> OnDisconnected;
        private bool writing = true;
        private bool reading = true;

        public NetworkPlayer(TcpClient tcpClient)
        {
            messageQueue = new ConcurrentQueue<NetworkMessage>();
            SpecificResponses = new ConcurrentDictionary<NetworkId, NetworkMessage>();
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
            while(writing)
            {
                if (!messageQueue.Any())
                {
                    Thread.Sleep(50);
                    continue;
                }

                NetworkMessage message;

                try
                {
                    if (messageQueue.TryDequeue(out message))
                    {
                        message.Send(stream);
                    }
                }
                catch (Exception e)
                {
                    Logger.Instance.Log(e.Message);
                    Disconnect(false);
                    return;
                }
            }
        }

        void ReadingThread()
        {
            while(reading)
            {
                byte[] data;
                using(var inputStream = new MemoryStream())
                {
                    byte[] key = new byte[sizeof(int)];
                    byte[] extra = new byte[sizeof(int)];
                    byte[] dataPrefix = new byte[sizeof(int)];
                    int receivedPrefixBytes;

                    try
                    {
                        var received = stream.Read(key, 0, sizeof(int));
                        if (received != 4)
                        {
                            Disconnect(false);
                            return;
                        }
                        received = stream.Read(extra, 0, sizeof(int));
                        if (received != 4)
                        {
                            Disconnect(false);
                            return;
                        }

                        if (BitConverter.ToInt32(key, 0) != NetworkMessage.KEY 
                            || BitConverter.ToInt32(extra, 0) != NetworkMessage.EXTRA)
                        {
                            Disconnect(false);
                            return;
                        }

                        receivedPrefixBytes = stream.Read(dataPrefix, 0, sizeof(int));
                    }
                    catch (Exception e)
                    {
                        Logger.Instance.Log(e.Message);
                        Disconnect(false);
                        return;
                    }

                    if (receivedPrefixBytes != 4)
                    {
                        Thread.Sleep(50);
                        continue;
                    }

                    int dataSize = BitConverter.ToInt32(dataPrefix, 0);
                    data = new byte[dataSize];

                    int bytesRemaining = dataSize;
                    int offset = 0;

                    var readTimer = Stopwatch.StartNew();
                    while (bytesRemaining > 0)
                    {
                        try
                        {
                            if (!reading)
                            {
                                return;
                            }

                            var readBytes = stream.Read(data, offset, bytesRemaining);
                            bytesRemaining -= readBytes;
                            offset += readBytes;
                        }
                        catch (Exception e)
                        {
                            Logger.Instance.Log(e.Message);
                            Disconnect(false);
                            return;
                        }
                    }

                    inputStream.Write(data, 0, dataSize);

                    string input = Encoding.UTF8.GetString(inputStream.ToArray(), 0, (int)inputStream.Length);

                    HandleMessage(input);
                }
            }
        }

        private void HandleMessage(string input)
        {
            var message = Serializer.Deserialize<NetworkMessage>(input);
            message.Received = DateTime.Now;

            if (message.MessageType == MessageTypes.Connected)
            {
                Id = (NetworkId)message.Data;
            }
            if (message.MessageType == MessageTypes.Disconnect)
            {
                Disconnect(false);
            }
            else if (message.ResponseTo != null && !message.ResponseTo.Value.Equals(NetworkId.Empty))
            {
                SpecificResponses.TryAdd(message.ResponseTo, message);
            }
            else
            {
                DataReceived?.Invoke(this, new NetworkDataRecievedEventArgs(message));
            }
        }

        public void Disconnect(bool sendDisconnectMessage)
        {
            if (sendDisconnectMessage)
            {
                SendAndWaitForDisconnect();
            }

            reading = false;
            writing = false;
            Thread.Sleep(16);
            stream.Close();
            client.Close();

            OnDisconnected?.Invoke(this, Id);
        }

        private void SendAndWaitForDisconnect()
        {
            messageQueue.Enqueue(new NetworkMessage(MessageTypes.Disconnect, null, Id, NetworkId.Generate()));
            
            while (messageQueue.Any())
            {
                Thread.Sleep(50);
            }
        }

        public T SendAndWaitForResponse<T>(NetworkMessage message)
        {
            Send(message);

            while (true)
            {
                Thread.Sleep(100);
                
                if (SpecificResponses.TryGetValue(message.MessageId, out NetworkMessage response))
                {
                    return (T)response.Data;
                }
            }
        }

        public NetworkId Id { get; set; }
        public string Name { get; set; }
        public ConcurrentDictionary<NetworkId, NetworkMessage> SpecificResponses { get; private set; }
    }
}
