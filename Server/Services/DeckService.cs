using DataLayer;
using DataLayer.Queries;
using Entities;
using NetworkingCore;
using System.Linq;

namespace Server.Services
{
    internal class DeckService : IService
    {
        public LongResult SaveDeck(DeckInfo deck)
        {
            if (deck.Id == 0)
            {
                Database.Instance.Insert(deck);
                Database.Instance.InsertList(deck.Cards);

                var query = new SelectQuery<DeckInfo>().AndEquals(nameof(deck.Name), deck.Name).AndEquals(nameof(deck.UserId), deck.UserId.ToString()).Limit(1);
                return Database.Instance.Select(query).First().Id;
            }

            Database.Instance.Update(deck);
            Database.Instance.ExecuteNonQuery($"DELETE FROM {new DeckCard().GetTableName()} WHERE {nameof(DeckCard.DeckId)} = {deck.Id}");
            Database.Instance.InsertList(deck.Cards);
            return deck.Id;
        }
    }
}
