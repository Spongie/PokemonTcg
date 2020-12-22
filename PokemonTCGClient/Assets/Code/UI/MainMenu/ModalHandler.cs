using TMPro;
using UnityEngine;

namespace Assets.Code.UI.MainMenu
{
    public class ModalHandler : MonoBehaviour
    {
        public TextMeshProUGUI Text;

        public static ModalHandler Instance;

        private void Awake()
        {
            Instance = this;
            gameObject.SetActive(false);
        }

        public void DisplayMessage(string message)
        {
            gameObject.SetActive(true);
            Text.text = message;
        }
    }
}
