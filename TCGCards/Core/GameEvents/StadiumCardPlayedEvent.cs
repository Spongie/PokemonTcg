using NetworkingCore;

namespace TCGCards.Core.GameEvents
{
    public class StadiumCardPlayedEvent : Event
    {
        public StadiumCardPlayedEvent()
        {
            GameEvent = GameEventType.StadiumCardPlayed;
        }

        public NetworkId Player { get; set; }
        public TrainerCard Card { get; set; }
    }
}
