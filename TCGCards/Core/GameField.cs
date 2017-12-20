using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Game
{
    public class GameField
    {
        public List<Player> Players;
        public Player ActivePlayer;
        public int Mode;

        public GameField()
        {
            Players = new List<Player>();
            Players.Add(new Player());
            Players.Add(new Player());
            ActivePlayer = Players[0];
        }
    }
}
