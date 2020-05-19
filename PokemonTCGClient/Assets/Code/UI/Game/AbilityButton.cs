using TCGCards.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.UI.Game
{
    public class AbilityButton : MonoBehaviour
    {
        private Ability ability;
        public Text abilityName;

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
