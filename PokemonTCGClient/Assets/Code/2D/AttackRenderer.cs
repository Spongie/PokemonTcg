using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TCGCards;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AttackRenderer : MonoBehaviour, IPointerClickHandler
{
    private Dictionary<EnergyTypes, Sprite> icons;
    public Texture2D icon_atlas;
    public GameObject costPrefab;
    public GameObject costParent;
    public GameObject description;
    public Text attackName;
    public Text damage;

    private void Awake()
    {
        icons = new Dictionary<EnergyTypes, Sprite>();

        foreach (var sprite in Resources.LoadAll<Sprite>(icon_atlas.name))
        {
            var type = (EnergyTypes)Enum.Parse(typeof(EnergyTypes), sprite.name);
            icons.Add(type, sprite);
        }
    }

    public void SetAttack(Attack attack)
    {
        description.SetActive(!string.IsNullOrEmpty(attack.Description));
        attackName.text = attack.Name;
        damage.text = attack.DamageText;

        foreach (var cost in attack.Cost)
        {
            for (int i = 0; i < cost.Amount; i++)
            {
                Instantiate(costPrefab, costParent.transform).GetComponent<Image>().sprite = icons[cost.EnergyType];
            }
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("Mouse down on: " + attackName.text);
    }

    public void AttackClicked()
    {
        Debug.Log("Clicked on: " + attackName.text);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked handler on: " + attackName.text);
    }
}
