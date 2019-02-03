using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCGCards;
using TCGCards.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityRenderer : MonoBehaviour
{
    public TextMeshProUGUI abilityName;
    public TextMeshProUGUI abilityDescription;
    private Ability ability;

    public void SetAbility(Ability ability)
    {
        this.ability = ability;
        abilityName.text = ability.Name;
        abilityDescription.text = ability.Description;

        if (ability.TriggerType != TriggerType.Activation && ability.CanActivate())
        {
            GetComponent<Button>().interactable = false;
        }
    }

    public void AttackClicked()
    {
        Debug.Log("Clicked on: " + abilityName.text);
    }
}
