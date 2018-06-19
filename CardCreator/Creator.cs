using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace CardCreator
{
    public class Creator
    {
        private const string baseUrl = "https://pkmncards.com/set/";

        public void Run(string setUrl, string setName)
        {
            var web = new HtmlWeb();
            var document = web.Load(baseUrl + setUrl);

            EmptyOrCreateDirectories(setName);

            foreach (var pokemon in getPokemonCardNodes(document))
            {
                processPokemon(pokemon, setName);
            }
        }

        private static void EmptyOrCreateDirectories(string setName)
        {
            if (Directory.Exists(setName))
            {
                Directory.Delete(setName, true);
                while (Directory.Exists(setName))
                {
                    Thread.Sleep(50);
                }
            }

            Directory.CreateDirectory(setName);
            Directory.CreateDirectory(setName + "/PokemonCards/");
            Directory.CreateDirectory(setName + "/Attacks/");
        }

        private IEnumerable<HtmlNode> getPokemonCardNodes(HtmlDocument document)
        {
            var contentNodes = document.DocumentNode.Descendants("Article")
                .Where(element => element.Descendants("div").Where(e => e.HasClass("card-type")).Where(x => x.InnerHtml.Contains("Pokémon")).Any());

            return contentNodes.Select(x => x.Descendants("a").FirstOrDefault()).ToList();
        }

        private void processPokemon(HtmlNode pokemonNode, string setName)
        {
            var web = new HtmlWeb();
            var document = web.Load(pokemonNode.Attributes["href"].Value);
            var textNodes = document.DocumentNode.Descendants("Article").FirstOrDefault()
                    .SelectNodes("//div")
                    .Where(node => node.HasClass("text") && node.Id.Contains("text"))
                    .First()
                    .ChildNodes;

            var name = textNodes[0].ChildNodes.Descendants("span").Last().InnerHtml;

            if (File.Exists($"{setName}/PokemonCards/{generateFileName(name)}.cs"))
            {
                Console.WriteLine($"{name} already processed, skipping...");
                return;
            }

            Console.WriteLine("Processing: " + name);

            var type = HttpUtility.HtmlDecode(textNodes[0].ChildNodes[1].InnerHtml).Split('–')[1].Trim();
            var hp = HttpUtility.HtmlDecode(textNodes[0].ChildNodes[1].InnerHtml).Split('–')[2].Replace("HP", string.Empty).Trim();
            var evolvesFrom = textNodes[0].ChildNodes[3].InnerHtml == "Basic" ? "" : textNodes[0].ChildNodes[4].InnerHtml;
            var attacks = new List<Attack>();
            WeaknessResistenceInfo weaknessResistenceInfo = null;
            bool hasPokemonPower = false;

            foreach (var node in textNodes.Skip(1))
            {
                if (IsAttackNode(node))
                {
                    var attack = Attack.Parse(node);
                    attacks.Add(attack);
                }
                else if (IsWeaknessResistanceNode(node))
                {
                    weaknessResistenceInfo = WeaknessResistenceInfo.Parse(node);
                }
                else if (IsPokemonPowerNode(node))
                {
                    hasPokemonPower = true;
                }
            }

            var pokemonCard = new PokemonCard
            {
                Hp = int.Parse(hp),
                Attacks = attacks,
                EvolvesFrom = evolvesFrom,
                Name = name,
                Type = type,
                WeaknessResistenceInfo = weaknessResistenceInfo,
                HasPokemonPower = hasPokemonPower
            };

            var code = pokemonCard.buildFromTemplate(setName);
            File.WriteAllText($"{setName}/PokemonCards/{generateFileName(name)}.cs", code);

            foreach (var attack in attacks)
            {
                string attackCode = attack.generateCode(setName);
                File.WriteAllText($"{setName}/Attacks/{generateFileName(attack.Name)}.cs", attackCode);
            }
        }

        private static string generateFileName(string name)
        {
            return name.Replace("é", "e").Replace("’", "").Replace("!", string.Empty).Replace(" ", string.Empty);
        }

        private static bool IsAttackNode(HtmlNode node)
        {
            if (!node.ChildNodes.Any())
            {
                return false;
            }

            return node.ChildNodes[0].InnerHtml.StartsWith("[");
        }

        private static bool IsWeaknessResistanceNode(HtmlNode node)
        {
            if (!node.ChildNodes.Any())
            {
                return false;
            }

            return node.ChildNodes[0].InnerHtml.StartsWith("Weakness");
        }

        private static bool IsPokemonPowerNode(HtmlNode node)
        {
            if (!node.ChildNodes.Any())
            {
                return false;
            }

            return node.ChildNodes[0].InnerHtml.StartsWith("Pokémon Power");
        }
    }
}
