using HtmlAgilityPack;
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
            var sets = FindSets();

            Console.WriteLine($"Found {sets.Count} sets");

            for(int i = 0; i < sets.Count; i++)
            {
                var set = sets[i];

                Console.WriteLine($"Downloading set {i + 1} of {sets.Count} - {set.Name}");

                var web = new HtmlWeb();
                var document = web.Load(set.Link);

                var contentNodes = document.DocumentNode.Descendants("Article").ToList();

                DownloadAllPokemons(contentNodes, set);
            }

            Console.Read();
        }

        static List<PokemonSet> FindSets()
        {
            var web = new HtmlWeb();
            var document = web.Load("https://pkmncards.com/sets/");

            return document.DocumentNode.Descendants("Article").First().Descendants("a").Skip(1).Select(x => new PokemonSet(x)).ToList();
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
                string fileName = baseFolder + "\\" + GetPokemonFileName(pokemon);

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

        private static string GetPokemonFileName(HtmlNode pokemon)
        {
            return pokemon.InnerHtml.Replace(" ", "")
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
