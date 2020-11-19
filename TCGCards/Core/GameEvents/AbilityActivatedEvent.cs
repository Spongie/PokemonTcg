using NetworkingCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TCGCards.Core.GameEvents
{
    public class AbilityActivatedEvent : Event
    {
        public AbilityActivatedEvent()
        {
            GameEvent = GameEventType.PokemonActivatesAbility;
        }

        public NetworkId PokemonId { get; set; }
    }
}
