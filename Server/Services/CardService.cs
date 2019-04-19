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
                var constructor = typeInfo.DeclaredConstructors.First();
                var parameters = new List<object>();

                for (int i = 0; i < constructor.GetParameters().Length; i++)
                {
                    parameters.Add(null);
                }

                cards.Add((Card)constructor.Invoke(parameters.ToArray()));
            }

            Logger.Instance.Log($"Loaded {cards.Count} to cache");
        }
    }
}
