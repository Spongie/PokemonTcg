using System;
using System.Diagnostics;
using System.Linq;

namespace ServerController
{
    public class Controller
    {
        public void Start()
        {
            var process = Process.GetProcessesByName("Server").FirstOrDefault();

            if (process == null)
            {
                Console.WriteLine("Starting server...");
                Process.Start("Server");
            }
        }

        public void Stop()
        {
            var process = Process.GetProcessesByName("Server").FirstOrDefault();

            if (process == null)
            {
                return;
            }

            Console.WriteLine("Stopping server...");
            process.Kill();
            process.WaitForExit();
        }

        public void Restart()
        {
            Stop();
            Start();
        }
    }
}
