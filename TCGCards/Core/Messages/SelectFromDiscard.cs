﻿using NetworkingCore;

namespace TCGCards.Core.Messages
{
    public class SelectFromDiscard : AbstractNetworkMessage
    {
        public SelectFromDiscard(int count, IDeckFilter filter)
        {
            Count = count;
            Filter = filter;
        }

        public int Count { get; set; }
        public IDeckFilter Filter { get; set; }
    }
}
