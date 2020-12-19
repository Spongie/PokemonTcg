using Entities;
using TCGCards.Core.Messages;
using UnityEngine;

namespace Assets.Code._2D
{
    public class SelectStatusPanel : MonoBehaviour
    {
        [Header("Buttons")]
        public GameObject SleepButton;
        public GameObject PoisonButton;
        public GameObject ParalyzeButton;
        public GameObject ConfuseButton;
        public GameObject BurnButton;

        public void OnSleepClick()
        {
            GameController.Instance.OnStatusSelected(StatusEffect.Sleep);
            gameObject.SetActive(false);
        }

        public void OnPoisonClick()
        {
            GameController.Instance.OnStatusSelected(StatusEffect.Poison);
            gameObject.SetActive(false);
        }

        public void OnParalyzeClick()
        {
            GameController.Instance.OnStatusSelected(StatusEffect.Paralyze);
            gameObject.SetActive(false);
        }

        public void OnConfuseClick()
        {
            GameController.Instance.OnStatusSelected(StatusEffect.Confuse);
            gameObject.SetActive(false);
        }

        public void OnBurnClick()
        {
            GameController.Instance.OnStatusSelected(StatusEffect.Burn);
            gameObject.SetActive(false);
        }

        internal void Init(PickStatusMessage message)
        {
            SleepButton.SetActive(message.CanSleep);
            ParalyzeButton.SetActive(message.CanParalyze);
            PoisonButton.SetActive(message.CanPoison);
            ConfuseButton.SetActive(message.CanConfuse);
            BurnButton.SetActive(message.CanBurn);
        }
    }
}
