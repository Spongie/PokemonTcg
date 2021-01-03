using System;
using TCGCards.Core.Messages;
using TMPro;
using UnityEngine;

namespace Assets.Code._2D
{
    public class EnterAmountPanel : MonoBehaviour
    {
        public TextMeshProUGUI Info;
        public TMP_InputField Input;

        public void OnDoneClicked()
        {
            string text = Input.text;
            int value;

            if (!int.TryParse(text, out value))
            {
                value = 0;
            }

            NetworkManager.Instance.SendToServer(new AskForAmountMessage() { Answer = value }, true);
            GameController.Instance.SpecialState = SpecialGameState.None;
            GameController.Instance.EnableButtons();
            gameObject.SetActive(false);
        }

        internal void Init(AskForAmountMessage amountMessage)
        {
            Info.text = amountMessage.Info;
        }
    }
}
