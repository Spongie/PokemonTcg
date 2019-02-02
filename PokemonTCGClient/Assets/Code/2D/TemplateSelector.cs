using System;
using System.Collections.Generic;
using System.Linq;
using TCGCards;
using UnityEngine;
using UnityEngine.UI;

public class TemplateSelector : MonoBehaviour
{
    public List<Sprite> templateMap;

    public void SetTemplate(EnergyTypes type)
    {
        string typeName = Enum.GetName(typeof(EnergyTypes), type);

        GetComponent<Image>().sprite = templateMap.FirstOrDefault(sprite => sprite.name == typeName);
    }
}