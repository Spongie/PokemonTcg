using PokemonTcgSdk;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace CardCreator
{
    public class Creator
    {
        public void Run(string setName, string namespaceName)
        {
            Debugger.Launch();
            if (Directory.Exists(namespaceName))
            {
                Directory.Delete(namespaceName, true);
                while (Directory.Exists(namespaceName))
                {
                    Thread.Sleep(10);
                }
            }

            Directory.CreateDirectory(namespaceName);
            Directory.CreateDirectory(namespaceName + "/Attacks");
            Directory.CreateDirectory(namespaceName + "/PokemonCards");

            var set = Sets.Find(new Dictionary<string, string>
            {
                { "name", setName }
            }).FirstOrDefault();

            if (set == null)
            {
                Console.WriteLine($"Set {setName} not found...");
                return;
            }

            Console.WriteLine("Fetching pokemon cards...");

            var cards = Card.Get<Pokemon>(new Dictionary<string, string>
            {
                { "setCode", set.Code },
                { "supertype", "Pokémon" }
            }).Cards;

            foreach (var card in cards)
            {
                var pokemon = new CreatorPokemonCard(card);
                Console.WriteLine("Processing " + pokemon.Name);
                string code = pokemon.buildFromTemplate(namespaceName);

                File.WriteAllText(namespaceName + "/PokemonCards/" + pokemon.ClassName + ".cs", code);
                foreach (var attack in pokemon.Attacks)
                {
                    File.WriteAllText(namespaceName + "/Attacks/" + attack.ClassName + ".cs", attack.generateCode(namespaceName));
                }
            }
        }
    }
}
