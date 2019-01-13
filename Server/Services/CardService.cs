﻿using Entities;
using System.Collections.Generic;

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

            //TODO Make this something good (Reflection)

            return this.cardSets;
        }

        public List<Format> GetFormats()
        {
            if (formats != null)
            {
                return formats;
            }

            //TODO Make this something good (Reflection)

            return formats;
        }

        public void InitTypes()
        {
            //TODO
        }
    }
}
