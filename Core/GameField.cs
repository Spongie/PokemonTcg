using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class GameField : MonoBehaviour
    {
        public List<Player> Players;
        public Player ActivePlayer;
        public int Mode;

        private void Start()
        {
            Players = new List<Player>();
            Players.Add(new Player());
            Players.Add(new Player());
            ActivePlayer = Players[0];
        }
    }
}
