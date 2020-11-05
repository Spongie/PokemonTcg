using Assets.Code._2D;
using Entities;
using TCGCards;
using TCGCards.Core.Messages;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.UI.Game
{
    public class SelectColorPanel : MonoBehaviour
    {
        private EnergyResourceManager energyResources;
        public GameObject buttonHolder;

        public void Init(string message)
        {
            GetComponent<Text>().text = message;
        }

        private void Start()
        {
            energyResources = GameObject.FindGameObjectWithTag("_global_").GetComponent<EnergyResourceManager>();

            foreach (var key in energyResources.Icons.Keys)
            {
                var child = new GameObject();
                child.transform.SetParent(buttonHolder.transform);
                Button button = child.AddComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    onClick(key);
                });
                child.AddComponent<Image>().sprite = energyResources.Icons[key];
                child.AddComponent<LayoutElement>();
            }
        }

        private void onClick(EnergyTypes energyType)
        {
            var message = new SelectColorMessage(energyType).ToNetworkMessage(NetworkManager.Instance.Me.Id);
            message.ResponseTo = NetworkManager.Instance.RespondingTo;

            NetworkManager.Instance.Me.Send(message);

            NetworkManager.Instance.RespondingTo = null;
            GameController.Instance.OnCardPicked();
            gameObject.SetActive(false);
        }
    }
}
