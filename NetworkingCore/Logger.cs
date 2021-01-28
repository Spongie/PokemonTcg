using System;
using System.IO;

namespace NetworkingCore
{
    public class Logger
    {
        private static Logger instance;

        private Logger()
        {

        }

        public static Logger Instance
        {
            get
            {
                if (instance == null)
                    instance = new Logger();

                return instance;
            }
        }

        public void Info(string info)
        {
            Console.WriteLine(info);
        }

        public void Error(string info)
        {
            File.AppendAllText("error.log", DateTime.Now + "  -  " + info + Environment.NewLine + Environment.NewLine);
        }
    }
}
