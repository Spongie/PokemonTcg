using NetworkingCore;

namespace TCGCards.Core.GameEvents
{
    public class TrainerCardPlayed : Event
    {
        public TrainerCardPlayed(GameFieldInfo gameField) : base(gameField)
        {
        }

        public NetworkId Player { get; set; }
        public TrainerCard Card { get; set; }
    }

    
}
