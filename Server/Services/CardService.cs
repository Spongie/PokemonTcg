using DataLayer;
using DataLayer.Queries;
using Entities;
using NetworkingCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Services
{
    public class CardService : IService
    {
        private List<CardSet> cardSets;
        private List<Format> formats;

        public List<CardSet> GetCardSets()
        {
            if (this.cardSets != null)
            {
                return this.cardSets;
            }
          
            var cardSets = Database.Instance.Select<CardSet>().ToList();
            var newSets = new List<CardSet>();

            foreach (var setType in TypeLoader.GetLoadedTypesAssignableFrom<ISet>())
            {
                var set = (ISet)Activator.CreateInstance(setType);
                
                if (!cardSets.Any(s => s.SetId == set.GetId()))
                {
                    newSets.Add(CardSet.CreateFrom(set));    
                }
            }

            if (newSets.Any())
            {
                Database.Instance.InsertList(newSets);
                cardSets = Database.Instance.Select<CardSet>().ToList();
            }

            foreach (var cardSet in cardSets)
            {
                cardSet.Cards = Database.Instance.Select(new SelectQuery<CardInfo>().AndEquals(nameof(CardInfo.SetId), cardSet.Id.ToString())).ToList();
            }

            this.cardSets = cardSets;

            return this.cardSets;
        }

        public List<Format> GetFormats()
        {
            if (formats != null)
            {
                return formats;
            }

            formats = Database.Instance.Select<Format>().ToList();

            foreach (var formatType in TypeLoader.GetLoadedTypesAssignableFrom<IFormat>())
            {
                var format = (IFormat)Activator.CreateInstance(formatType);
                var bannedCards = Database.Instance.Select(new SelectQuery<CardInfo>().AndIn(nameof(CardInfo.ClassName), format.GetBannedCardsClassNames()));
                var sets = Database.Instance.Select(new SelectQuery<CardSet>().AndIn(nameof(CardSet.Id), format.GetSetIds().Select(x => x.ToString())));

                Database.Instance.Insert(Format.CreateFrom(format));

                Database.Instance.InsertList(bannedCards.Select(x => new BannedCard { FormatId = format.GetFormatId(), CardId = x.Id }));
                Database.Instance.InsertList(sets.Select(x => new FormatSet { FormatId = format.GetFormatId(), SetId = x.SetId }));
            }

            foreach (var format in formats)
            {
                format.BannedCards = Database.Instance.Select(new SelectQuery<BannedCard>().AndEquals(nameof(BannedCard.FormatId), format.Id.ToString())).ToList();
                format.Sets = Database.Instance.Select(new SelectQuery<FormatSet>().AndEquals(nameof(FormatSet.FormatId), format.Id.ToString())).ToList();
            }

            return formats;
        }
    }
}
