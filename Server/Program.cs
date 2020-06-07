using System;
using System.Threading;

namespace Server
{
    class Program
    {
        private static MasterServer server;

        static void Main(string[] args)
        {
            var t = new Thread(RunServer);
            t.Start();

            while (true)
            {
                if (server == null)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                server.PrintInfo();

                Thread.Sleep(1000);
            }
        }

        private static void RunServer(object obj)
        {
            server = new MasterServer();
            server.Start(80);
        }
    }
}
