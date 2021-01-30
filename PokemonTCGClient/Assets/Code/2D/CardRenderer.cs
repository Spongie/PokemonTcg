using System.Collections;
using TCGCards;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using Assets.Code;
using Assets.Code.UI.Game;
using Assets.Code.UI.Gameplay;
using TMPro;
using Assets.Code._2D;
using Entities;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Assets.Code._2D.GameCard;

public class CardRenderer : MonoBehaviour, IPointerClickHandler
{
    public PokemonCard pokemon;

    public Image art;
    public Card card;
    public bool isSelected;
    public GameObject overlay;
    public CardPopupHandler popupHandler;
    public TextMeshProUGUI DamageDisplay;
    public AttachedCardPanel attachedCardPanel;

    [Header("Status Images")]
    public GameObject PoisonIcon;
    public GameObject BurnedIcon;
    public GameObject ParalyzedIcon;
    public GameObject AsleepIcon;
    public GameObject ConfusedIcon;

    public TextMeshProUGUI damagePreviewText;
    
    [Header("Attached Energy")]
    public GameObject AttachedEnergyList;
    public GameObject AttachedEnergyPrefab;
    public Sprite FireIcon;
    public Sprite WaterIcon;
    public Sprite ElectricIcon;
    public Sprite GrassIcon;
    public Sprite FightingIcon;
    public Sprite PsychicIcon;
    public Sprite DoubleColorlessIcon;
    public Sprite SpecialIcon;
    
    [Header("Effects")]
    public Transform EffectParent;
    public GameObject WaterEffect;
    public GameObject ElectricEffect;
    public GameObject RockEffect;
    public GameObject FightingEffect;
    public GameObject FireEffect;
    public GameObject GrassEffect;
    public GameObject PsychicEffect;
    public GameObject ColorlessEffect;
    public GameObject DarknessEffect;
    public GameObject DragonEffect;
    public GameObject SteelEffect;
    public GameObject Abilityeffect;
    public GameObject EvolveEffect;
    public GameObject DeathEffect;
    public GameObject BounceEffect;
    public GameObject HealEffect;

    private Zoomer zoomer;

    void Awake()
    {
        zoomer = GetComponent<Zoomer>();
    }

    public void SetCard(Card card, bool spawnAttachedEnergy, bool isPreview = false)
    {
        this.card = card;

        if (card == null)
        {
            art.color = new Color(art.color.r, art.color.g, art.color.b, 0);
            return;
        }

        art.color = new Color(art.color.r, art.color.g, art.color.b, 1);

        if (card is PokemonCard)
        {
            pokemon = (PokemonCard)card;
        }

        if (!isPreview)
        {
            GameController.Instance.AddCard(this);
        }

        if (card is PokemonCard)
        {
            var pokemon = (PokemonCard)card;

            if (spawnAttachedEnergy)
            {
                SpawnAttachedEnergy(pokemon.AttachedEnergy);
            }

            attachedCardPanel.SetAttachments(pokemon.AttachedTools);

            if (pokemon.DamageCounters > 0)
            {
                DamageDisplay.text = pokemon.DamageCounters.ToString();
            }
            else
            {
                DamageDisplay.text = string.Empty;
            }

            EnableStatusIcons();
        }

        StartCoroutine(LoadSprite(card));
    }

    internal void EnableStatusIcons()
    {
        PoisonIcon.SetActive(pokemon.IsPoisoned);
        AsleepIcon.SetActive(pokemon.IsAsleep);
        ParalyzedIcon.SetActive(pokemon.IsParalyzed);
        ConfusedIcon.SetActive(pokemon.IsConfused);
        BurnedIcon.SetActive(pokemon.IsBurned);
    }

    private void SpawnAttachedEnergy(List<EnergyCard> attachedEnergyCards)
    {
        if (attachedEnergyCards == null)
        {
            return;
        }

        foreach (var attachedEnergy in attachedEnergyCards)
        {
            var attachedObject = Instantiate(AttachedEnergyPrefab, AttachedEnergyList.transform);
            attachedObject.GetComponent<AttachedEnergy>().energyCard = attachedEnergy;
            attachedObject.GetComponent<Image>().sprite = EnergyResourceManager.Instance.GetSpriteForEnergyCard(attachedEnergy);
        }
    }

    internal void insertAttachedEnergy(EnergyCard attachedEnergy)
    {
        var attachedObject = Instantiate(AttachedEnergyPrefab, AttachedEnergyList.transform);
        attachedObject.GetComponent<AttachedEnergy>().energyCard = attachedEnergy;
        attachedObject.GetComponent<Image>().sprite = EnergyResourceManager.Instance.GetSpriteForEnergyCard(attachedEnergy);
    }

    IEnumerator LoadSprite(Card card)
    {
        yield return CardImageLoader.Instance.LoadSpriteRoutine(card, art);
    }

    public void SpawnAbilityEffect()
    {
        Instantiate(Abilityeffect, EffectParent);
    }

    public void SpawnEvolveEffect()
    {
        Instantiate(EvolveEffect, EffectParent);
    }

    public void SpawnDeathEffect()
    {
        Instantiate(DeathEffect, EffectParent);
    }

    public void SpawnHealEffect()
    {
        Instantiate(HealEffect, EffectParent);
    }

    internal void SpawnBounceEffect()
    {
        Instantiate(BounceEffect, EffectParent);
    }

    internal void SpawnDamageEffect(EnergyTypes damageType)
    {
        GameObject effectPrefab;

        switch (damageType)
        {
            case EnergyTypes.Colorless:
                effectPrefab = ColorlessEffect;
                break;
            case EnergyTypes.Water:
                effectPrefab = WaterEffect;
                break;
            case EnergyTypes.Fire:
                effectPrefab = FireEffect;
                break;
            case EnergyTypes.Grass:
                effectPrefab = GrassEffect;
                break;
            case EnergyTypes.Electric:
                effectPrefab = ElectricEffect;
                break;
            case EnergyTypes.Psychic:
                effectPrefab = PsychicEffect;
                break;
            case EnergyTypes.Fighting:
                effectPrefab = RockEffect;
                break;
            case EnergyTypes.Darkness:
                effectPrefab = DarknessEffect;
                break;
            case EnergyTypes.Steel:
                effectPrefab = SteelEffect;
                break;
            case EnergyTypes.Fairy:
                effectPrefab = ColorlessEffect;
                break;
            case EnergyTypes.Dragon:
                effectPrefab = DragonEffect;
                break;
            default:
                effectPrefab = ColorlessEffect;
                break;
        }

        Instantiate(effectPrefab, EffectParent);
    }

    public void StartOnDamageTaken(int damage)
    {
        StartCoroutine(DisplayDamageRoutine(damage));
    }

    IEnumerator DisplayDamageRoutine(int damage)
    {
        damagePreviewText.text = damage.ToString();
        damagePreviewText.fontSize = 40;
        damagePreviewText.gameObject.SetActive(true);

        while (damagePreviewText.fontSize < 60)
        {
            damagePreviewText.fontSize += 1.25f;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(0.3f);
        damagePreviewText.gameObject.SetActive(false);
        damagePreviewText.fontSize = 30;

        var actualDamageText = DamageDisplay.GetComponentInChildren<TextMeshProUGUI>();
        int currentDamage = 0;

        if (!string.IsNullOrEmpty(actualDamageText.text.Trim()))
        {
            currentDamage = int.Parse(actualDamageText.text.Trim());
        }

        actualDamageText.text = (currentDamage + damage).ToString();
    }

    internal void FadeOut()
    {
        art.color = new Color(art.color.r, art.color.g, art.color.b, 0.5f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        if (GameController.Instance.SpecialState == SpecialGameState.SelectingColor ||
            GameController.Instance.SpecialState == SpecialGameState.SelectEnergyToRetreat ||
            GameController.Instance.SpecialState == SpecialGameState.SelectingPrize ||
            GameController.Instance.SpecialState == SpecialGameState.SelectingFromList)
        {
            GameController.Instance.selectFromListPanel.GetComponent<SelectFromListPanel>().OnCardClicked(this);
        }
        else
        {
            GameController.Instance.OnCardClicked(this);
        }
    }

    internal void SetIsActivePokemon()
    {
        zoomer.SetPivotForGame();
    }

    internal void SetIsBenched()
    {
        zoomer.SetPivotForGame();
    }

    internal void SetSelected(bool selected)
    {
        isSelected = selected;
        overlay.SetActive(isSelected);
    }

    public void DisplayPopup()
    {
        if (!popupHandler.gameObject.activeSelf)
        {
            popupHandler.gameObject.SetActive(true);
            popupHandler.Init(card);
        }
    }
}
