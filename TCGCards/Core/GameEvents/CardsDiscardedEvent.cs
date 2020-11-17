﻿using NetworkingCore;
using System.Collections.Generic;

namespace TCGCards.Core.GameEvents
{
    public class CardsDiscardedEvent : Event
    {
        public CardsDiscardedEvent(GameFieldInfo gameField) : base(gameField)
        {
            GameEvent = GameEventType.DiscardsCard;
        }

        public NetworkId Player { get; set; }
        public List<Card> Cards { get; set; }
    }
}
