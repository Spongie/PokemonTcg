using Assets.Code._2D;
using Assets.Code.UI.Gameplay;
using System.Collections.Generic;
using TCGCards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.UI
{
    public class AttackButton : MonoBehaviour
    {
        private Attack attack;

        public GameObject costPrefab;
        public GameObject costGrid;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI damageText;

        public void Init(Attack attack)
        {
            var energyResources = GameObject.FindGameObjectWithTag("_global_").GetComponent<EnergyResourceManager>();

            this.attack = attack;
            nameText.text = attack.Name;
            damageText.text = attack.DamageText;

            foreach (var cost in attack.Cost)
            {
                for (int i = 0; i < cost.Amount; i++)
                {
                    var attackObject = Instantiate(costPrefab, costGrid.transform);
                    attackObject.GetComponent<Image>().sprite = energyResources.Icons[cost.EnergyType];
                }
            }
        }

        public void OnClick()
        {
            NetworkManager.Instance.gameService.Attack(GameController.Instance.gameField.Id, attack.Id);
            transform.parent.gameObject.GetComponent<CardPopupHandler>().ClearAttackButtons();
            transform.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
