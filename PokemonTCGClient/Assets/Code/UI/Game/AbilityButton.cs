using TCGCards.Core;
using TMPro;
using UnityEngine;

namespace Assets.Code.UI.Game
{
    public class AbilityButton : MonoBehaviour
    {
        private Ability ability;
        public TextMeshProUGUI abilityName;

        public void Init(Ability ability)
        {
            this.ability = ability;
            abilityName.text = ability.Name;
        }

        public void OnClick()
        {
            GameController.Instance.ActivateAbility(ability);
        }
    }
}
