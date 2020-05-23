using NetworkingCore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TCGCards;

namespace Server.Services
{
    public class CardService : IService
    {
        private List<Card> cards;

        public void InitTypes()
        {
            Logger.Instance.Log("Loading cards cache");

            cards = new List<Card>();

            foreach (var typeInfo in Assembly.GetExecutingAssembly().GetReferencedAssemblies().Select(Assembly.Load).SelectMany(x => x.DefinedTypes)
                .Where(type => typeof(Card).GetTypeInfo().IsAssignableFrom(type.AsType()) && !type.IsAbstract && type.Name != nameof(PokemonCard)))
            {
                cards.Add(Card.CreateFromTypeInfo(typeInfo));
            }

            Logger.Instance.Log($"Loaded {cards.Count} to cache");
        }
    }
}
