using Assets.Code;
using Assets.Code._2D;
using TCGCards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttackRenderer : MonoBehaviour
{
    public GameObject costPrefab;
    public GameObject costParent;
    public GameObject description;
    public TextMeshProUGUI attackName;
    public TextMeshProUGUI damage;
    private Attack attack;
    private EnergyResourceManager energyResources;

    private void Init()
    {
        energyResources = GameObject.FindGameObjectWithTag("_global_").GetComponent<EnergyResourceManager>();
    }

    public void SetAttack(Attack attack)
    {
        Init();
        this.attack = attack;
        description.SetActive(!string.IsNullOrEmpty(attack.Description));
        attackName.text = attack.Name;
        damage.text = attack.DamageText;

        foreach (var cost in attack.Cost)
        {
            for (int i = 0; i < cost.Amount; i++)
            {
                Instantiate(costPrefab, costParent.transform).GetComponent<Image>().sprite = energyResources.Icons[cost.EnergyType];
            }
        }
    }

    public void AttackClicked()
    {
        GameController.Instance.Attack(attack);
    }
}
