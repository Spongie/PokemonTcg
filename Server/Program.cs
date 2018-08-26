using DataLayer;
using System;
using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread t = new Thread(RunServer);
            t.Start();

            Console.Read();

            Database.Instance.Dispose();
        }

        private static void RunServer(object obj)
        {
            var server = new MasterServer();
            server.Start(1565);
        }
    }
}
