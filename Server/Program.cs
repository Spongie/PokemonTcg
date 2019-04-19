using System;
using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = new Thread(RunServer);
            t.Start();

            Console.Read();
        }

        private static void RunServer(object obj)
        {
            var server = new MasterServer();
            server.Start(1565);
        }
    }
}
