using System;

namespace CardCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("wrong amount of parameters");
                return;
            }

            new Creator().Run(args[0], args[1]);

            Console.WriteLine("Completed");
            Console.Read();
        }
    }
}
