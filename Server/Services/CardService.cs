using NetworkingCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using TCGCards;

namespace Server.Services
{
    public class CardService : IService
    {
        private List<Card> cards;

        public void InitTypes()
        {
            //Logger.Instance.Log("Loading cards cache");

            //cards = new List<Card>();


            //foreach (var cardFile in Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll"))
            //{
            //    try
            //    {
            //        var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(new FileInfo(cardFile).FullName);

            //        foreach (var typeInfo in assembly.DefinedTypes.Where(type => typeof(Card).GetTypeInfo().IsAssignableFrom(type.AsType()) && !type.IsAbstract && type.Name != nameof(PokemonCard)))
            //        {
            //            cards.Add(Card.CreateFromTypeInfo(typeInfo));
            //        }
            //    }
            //    catch
            //    {
            //        Console.WriteLine("Failed to load " + cardFile);
            //    }
            //}

            //Logger.Instance.Log($"Loaded {cards.Count} to cache");
        }
    }
}
