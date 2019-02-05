﻿using System;
using System.Collections.Generic;
using Assets.Code;
using TCGCards;
using UnityEngine;
using UnityEngine.UI;

public class AttackRenderer : MonoBehaviour
{
    private Dictionary<EnergyTypes, Sprite> icons;
    public Texture2D icon_atlas;
    public GameObject costPrefab;
    public GameObject costParent;
    public GameObject description;
    public Text attackName;
    public Text damage;
    private Attack attack;

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
        this.attack = attack;
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

    public void AttackClicked()
    {
        GameController.Instance.Attack(attack);
    }
}
