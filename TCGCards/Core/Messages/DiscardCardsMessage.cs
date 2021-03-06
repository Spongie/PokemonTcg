﻿using NetworkingCore;
using System.Collections.Generic;

namespace TCGCards.Core.Messages
{
    public class DiscardCardsMessage : AbstractNetworkMessage
    {
        public DiscardCardsMessage() :this(0)
        {

        }

        public DiscardCardsMessage(int count) :this(count, new List<IDeckFilter>())
        {

        }

        public DiscardCardsMessage(int count, List<IDeckFilter> filters)
        {
            MessageType = MessageTypes.DiscardCards;
            Count = count;
            Filters = new List<IDeckFilter>(filters);
        }

        public int MinCount { get; set; }
        public int Count { get; set; }
        public List<IDeckFilter> Filters { get; set; }
        public string Info { get; set; }
    }
}
