using Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCGCards.Core.GameEvents;
using UnityEngine;

namespace Assets.Code.UI.Events
{
    public class DamageTakenEventHandler : MonoBehaviour
    {
        public void Trigger(DamageTakenEvent damageTakenEvent)
        {
            var attackedCard = GameController.Instance.GetCardRendererById(damageTakenEvent.PokemonId);

            StartCoroutine(DamageTakenRoutine(attackedCard, damageTakenEvent.Damage, damageTakenEvent.DamageType));
        }

        IEnumerator DamageTakenRoutine(CardRenderer cardRenderer, int damage, EnergyTypes damageType)
        {
            cardRenderer.SpawnDamageEffect(damageType);
            cardRenderer.StartOnDamageTaken(damage);

            yield return new WaitForSeconds(0.4f);

            cardRenderer.damageTakenText.text = (int.Parse(cardRenderer.damageTakenText.text) + damage).ToString();
            GameEventHandler.Instance.EventCompleted();
        }
    }
}
