using NetworkingCore;
using System.Collections.Generic;

namespace TCGCards.Core.GameEvents
{
    public class GameFieldInfo
    {
        public PlayerInfo Opponent { get; set; }
        public PlayerInfo Me { get; set; }
        public List<Card> CardsInMyHand { get; set; }
        public NetworkId ActivePlayer { get; set; }
        public GameFieldState CurrentState { get; set; }
        public TrainerCard StadiumCard { get; set; }
    }
}
