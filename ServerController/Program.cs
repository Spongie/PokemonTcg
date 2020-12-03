using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace ServerController
{
    class Program
    {
        static void Main(string[] args)
        {
            var controller = new Controller();

            using(var listener = new HttpListener())
            {
                listener.Prefixes.Add("http://*:80/");

                listener.Start();
                Console.WriteLine("Waiting for instructions");

                while (true)
                {
                    HttpListenerContext context = listener.GetContext();

                    HttpListenerRequest request = context.Request;

                    string documentContents;
                    using (Stream receiveStream = request.InputStream)
                    {
                        using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                        {
                            documentContents = readStream.ReadToEnd();
                        }
                    }

                    var action = request.Url.ToString().Split("/").Last();
                    Console.WriteLine($"Received action {action} processing...");

                    if (action == "start")
                    {
                        controller.Start();
                    }
                    else if (action == "stop")
                    {
                        controller.Stop();
                    }
                    else if (action == "restart")
                    {
                        controller.Restart();
                    }

                    Console.WriteLine("Done...");

                    HttpListenerResponse response = context.Response;
                    string responseString = "<HTML><BODY> Success!</BODY></HTML>";
                    byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                    
                    response.ContentLength64 = buffer.Length;
                    Stream output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                }
            }
        }
    }
}
