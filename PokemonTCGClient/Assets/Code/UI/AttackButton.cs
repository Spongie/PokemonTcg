using Assets.Code._2D;
using TCGCards;
using TCGCards.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.UI
{
    public class AttackButton : MonoBehaviour
    {
        public GameObject costPrefab;
        public GameObject costGrid;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI damageText;

        public void Init(Attack attack)
        {
            var energyResources = GameObject.FindGameObjectWithTag("_global_").GetComponent<EnergyResourceManager>();

            nameText.text = attack.Name;
            damageText.text = attack.DamageText;

            foreach (var cost in attack.Cost)
            {
                for (int i = 0; i < cost.Amount; i++)
                {
                    Instantiate(costPrefab, costGrid.transform).GetComponent<Image>().sprite = energyResources.Icons[cost.EnergyType];
                }
            }
        }
    }
}
