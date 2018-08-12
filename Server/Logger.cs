using System;

namespace Server
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

        public void Log(string info)
        {
            Console.WriteLine(info);
        }
    }
}
