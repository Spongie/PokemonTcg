using Assets.Code._2D;
using Assets.Code.UI.Gameplay;
using System.Collections.Generic;
using System.Linq;
using TCGCards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.UI
{
    public struct GridLayoutSettings
    {
        public int Columns { get; set; }
        public Vector2 Size { get; set; }
        
    }

    public class AttackButton : MonoBehaviour
    {
        private Attack attack;

        public GameObject costPrefab;
        public GameObject costGridObject;
        public GridLayoutGroup costGrid;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI damageText;

        private static GridLayoutSettings[] cGridSettings = new[]
        {
            new GridLayoutSettings { Columns = 2, Size = new Vector2(15, 15) },
            new GridLayoutSettings { Columns = 2, Size = new Vector2(15, 15) },
            new GridLayoutSettings { Columns = 2, Size = new Vector2(15, 15) },
            new GridLayoutSettings { Columns = 2, Size = new Vector2(15, 15) },
            new GridLayoutSettings { Columns = 2, Size = new Vector2(15, 10) },
            new GridLayoutSettings { Columns = 3, Size = new Vector2(15, 10) }
        };

        public void Init(Attack attack)
        {
            var energyResources = GameObject.FindGameObjectWithTag("_global_").GetComponent<EnergyResourceManager>();

            var gridSettings = cGridSettings[attack.Cost.Sum(x => x.Amount)];
            costGrid.constraintCount = gridSettings.Columns;
            costGrid.cellSize = gridSettings.Size;

            this.attack = attack;
            nameText.text = attack.Name;
            damageText.text = attack.Damage.ToString();

            foreach (var cost in attack.Cost)
            {
                for (int i = 0; i < cost.Amount; i++)
                {
                    var attackObject = Instantiate(costPrefab, costGridObject.transform);
                    attackObject.GetComponent<Image>().sprite = energyResources.Icons[cost.EnergyType];
                }
            }
        }

        public void OnClick()
        {
            NetworkManager.Instance.gameService.Attack(GameController.Instance.gameField.Id, attack.Id);
            transform.parent.parent.gameObject.GetComponent<CardPopupHandler>().ClearAttackButtons();
            transform.parent.parent.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
