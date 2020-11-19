using NetworkingCore;

namespace TCGCards.Core.GameEvents
{
    public class TrainerCardPlayed : Event
    {
        public TrainerCardPlayed()
        {
            GameEvent = GameEventType.TrainerCardPlayed;
        }

        public NetworkId Player { get; set; }
        public TrainerCard Card { get; set; }
    }

    
}
