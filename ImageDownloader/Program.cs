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

            Parallel.ForEach(sets, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, (set) =>
            {
                int myId = Interlocked.Increment(ref totalIndex);
                int progress = 0;

                string baseFolder = set.Name.Replace(" ", "");

                if (!Directory.Exists(baseFolder))
                {
                    Directory.CreateDirectory(baseFolder);
                }

                var cards = Card.Get<Pokemon>(new Dictionary<string, string>
                {
                    { CardQueryTypes.SetCode, set.Code},
                    { CardQueryTypes.PageSize, "500" }
                }).Cards;
                
                float max = cards.Count;

                foreach (var card in cards)
                {
                    string fileName = Path.Combine(baseFolder, GetPokemonFileName(card));

                    if (File.Exists(fileName))
                    {
                        continue;
                    }

                    using (var client = new WebClient())
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
                    lock (lockObject)
                    {
                        Console.SetCursorPosition(0, myId);
                        Console.Write($"Downloading {set.Name.PadRight(25)} - {(progress / max).ToString("p2").PadLeft(7, '0')}");
                    }
                }
            });

            Console.Read();
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
