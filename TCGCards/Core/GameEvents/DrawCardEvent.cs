﻿using NetworkingCore;
using System.Collections.Generic;

namespace TCGCards.Core.GameEvents
{
    public class DrawCardsEvent : Event
    {
        public DrawCardsEvent()
        {
            GameEvent = GameEventType.DrawsCard;
        }

        public NetworkId Player { get; set; }
        public List<Card> Cards { get; set; }
    }
}
