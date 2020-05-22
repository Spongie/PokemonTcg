using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.UI.Game
{
    public class GameLogView : MonoBehaviour
    {
        public Text text;

        private void Update()
        {
            text.text = string.Join(Environment.NewLine, GameController.Instance.gameLog);    
        }
    }
}
