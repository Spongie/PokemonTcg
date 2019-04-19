using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using PokemonTcgSdk;
using PokemonTcgSdk.Models;

namespace CardDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            var sets = Sets.All();

            Console.WriteLine($"Found {sets.Count} sets");
            int totalIndex = 0;
            object lockObject = new object();
            int cursorStart = Console.CursorTop;

            foreach (var set in sets)
            {
                int myId = Interlocked.Increment(ref totalIndex);
                int progress = 0;

                string baseFolder = set.Name.Replace(" ", "");

                if (!Directory.Exists(baseFolder))
                {
                    Directory.CreateDirectory(baseFolder);
                }

                var response = Card.Get<Pokemon>(new Dictionary<string, string>
                {
                    { CardQueryTypes.SetCode, set.Code},
                    { CardQueryTypes.PageSize, "500" }
                });

                List<PokemonCard> cards;

                try
                {
                    cards = response.Cards;
                }
                catch (Exception)
                {
                    continue;
                }

                float max = cards.Count;

                Parallel.ForEach(cards, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * 2 }, (card) =>
                {
                    string fileName = Path.Combine(baseFolder, GetPokemonFileName(card));

                    if (File.Exists(fileName))
                    {
                        progress++;
                        UpdateProgress(set, lockObject, progress, max);
                        return;
                    }

                    DownloadCard(card, fileName);

                    progress++;
                    UpdateProgress(set, lockObject, progress, max);
                });

                Console.WriteLine("");
            }

            Console.WriteLine("Done!");
        }

        private static void DownloadCard(PokemonCard card, string fileName)
        {
            using (var client = new ImageWebRequest())
            {
                try
                {
                    client.DownloadFile(card.ImageUrlHiRes, fileName);
                }
                catch (WebException e)
                {
                    if (e.Message.Contains("404"))
                    {
                        try
                        {
                            client.DownloadFile(card.ImageUrl, fileName);
                        }
                        catch (Exception)
                        {
                            Console.Write("Failed to download image for: " + card.Name);
                        }
                    }
                    else
                    {
                        Console.Write("Failed to download image for: " + card.Name);
                    }
                }
            }
        }

        private static int UpdateProgress(SetData set, object lockObject, int progress, float max)
        {
            lock (lockObject)
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"Downloading {set.Name.PadRight(25)} - {progress}/{max}     ");
            }

            return progress;
        }

        class ImageWebRequest : WebClient
        {
            protected override WebRequest GetWebRequest(Uri address)
            {
                WebRequest request = base.GetWebRequest(address);
                request.Timeout = 10000;
                return request;
            }
        }

        private static void WriteStatusLine(float status)
        {
            Console.Write($"\rDownloading: {status.ToString("p2").PadLeft(7, '0')}");
        }

        private static string GetPokemonFileName(PokemonCard card)
        {
            return card.Name.Replace(" ", "")
                .Replace("'", "")
                .Replace("´", "")
                .Replace("`", "")
                .Replace("-", "")
                .Replace(":", "")
                .Replace("é", "e")
                .Replace("’", "")
                .Replace("?", "")
                .Replace("&#8217;", "") + ".png";
        }
    }
}
