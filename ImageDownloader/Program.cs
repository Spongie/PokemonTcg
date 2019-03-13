using HtmlAgilityPack;
using PokemonTcgSdk;
using PokemonTcgSdk.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

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

            Parallel.ForEach(sets, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, (set) =>
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
                } catch (Exception e)
                {
                    return;
                }

                float max = cards.Count;

                foreach (var card in cards)
                {
                    string fileName = Path.Combine(baseFolder, GetPokemonFileName(card));

                    if (File.Exists(fileName))
                    {
                        progress++;
                        UpdateProgress(set, lockObject, cursorStart + myId, progress, max);
                        continue;
                    }

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
                                    Console.WriteLine("Failed to download image for: " + card.Name);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Failed to download image for: " + card.Name);
                            }
                        }
                    }

                    progress++;
                    UpdateProgress(set, lockObject, cursorStart + myId, progress, max);
                }
            });

            Console.Read();
        }

        private static int UpdateProgress(SetData set, object lockObject, int myId, int progress, float max)
        {
            lock (lockObject)
            {
                Console.SetCursorPosition(0, myId);
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

        class PokemonSet
        {
            public string Name { get; set; }
            public string Link { get; set; }
        }

        static void DownloadAllPokemons(List<HtmlNode> htmlNodes, PokemonSet set)
        {
            string baseFolder = set.Name.Replace(" ", "");

            if(!Directory.Exists(baseFolder))
                Directory.CreateDirectory(baseFolder);

            var pokemons = htmlNodes.Select(x => x.Descendants("a").FirstOrDefault()).ToList();

            int current = 0;
            float max = pokemons.Count;

            Parallel.ForEach(pokemons, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, (pokemon) =>
            {
                string fileName = baseFolder + "\\";// + GetPokemonFileName(pokemon);

                if(new FileInfo(fileName).Exists)
                {
                    Interlocked.Increment(ref current);
                    var currentStatus = current / max;

                    WriteStatusLine(currentStatus);
                    return;
                }

                lock(string.Intern(fileName))
                {
                    if(!new FileInfo(fileName).Exists)
                    {
                        var web = new HtmlWeb();
                        var document = web.Load(pokemon.Attributes["href"].Value);
                        var imageNode = document.DocumentNode.Descendants("Article").Select(x => x.Descendants("img").FirstOrDefault()).FirstOrDefault();

                        using(var client = new WebClient())
                        {
                            try
                            {
                                client.DownloadFile(imageNode.Attributes["src"].Value, fileName);
                            }
                            catch
                            {
                                client.DownloadFile(imageNode.ParentNode.Attributes["href"].Value, fileName);
                            }
                        }
                    }
                }

                Interlocked.Increment(ref current);
                var status = current / max;

                WriteStatusLine(status);
            });

            Console.WriteLine();
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
                .Replace("&#8217;", "") + ".jpg";
        }
    }
}
