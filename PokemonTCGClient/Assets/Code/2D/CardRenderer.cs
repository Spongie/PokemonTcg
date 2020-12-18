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

public class CardRenderer : MonoBehaviour, IPointerClickHandler
{
    public static PokemonCard pokemon;

    public GameObject AttachedCardPrefab;

    public Image art;
    public Card card;
    public bool isActivePokemon;
    public bool isSelected;
    public GameObject overlay;
    public CardPopupHandler popupHandler;
    public TextMeshProUGUI DamageDisplay;
    public Transform AttachmentParent;
    public GameObject AttachmentPrefab;

    [Header("Status Images")]
    public GameObject PoisonIcon;
    public GameObject BurnedIcon;
    public GameObject ParalyzedIcon;
    public GameObject AsleepIcon;
    public GameObject ConfusedIcon;

    public TextMeshProUGUI damageTakenText;
    
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

    void Start()
    {
        //SetCard(new PokemonCard() 
        //{ 
        //    SetCode = "base1", 
        //    IsRevealed = true, 
        //    ImageUrl = "https://images.pokemontcg.io/base1/33_hires.png", 
        //    Attacks = new ObservableCollection<Attack> { new Attack() { Name = "Attacken"} },
        //    AttachedTools = new List<TrainerCard>
        //    {
        //        new TrainerCard {IsRevealed =true, ImageUrl = "https://images.pokemontcg.io/base1/80_hires.png", SetCode = "base1" },
        //        new TrainerCard {IsRevealed =true, ImageUrl = "https://images.pokemontcg.io/base1/84_hires.png", SetCode = "base1" },
        //    }
        //}, ZoomMode.Center, true);
        //GameController.Instance.AddCard(this);
        //SetCard(new Bulbasaur(null) { IsRevealed = true }, ZoomMode.Center);
    }

    public ZoomMode GetZoomMode()
    {
        var zoomer = GetComponentInChildren<CardZoomer>();

        if (zoomer != null)
        {
            return zoomer.zoomMode;
        }

        return ZoomMode.Center;
    }

    public void SetZoomMode(ZoomMode zoomMode)
    {
        var zoomer = GetComponentInChildren<CardZoomer>();

        if (zoomer != null)
        {
            zoomer.zoomMode = zoomMode;
        }
    }

    public void SetCard(Card card, ZoomMode zoomMode, bool spawnAttachedEnergy, bool isPreview = false)
    {
        if (card is PokemonCard)
        {
            pokemon = (PokemonCard)card;
        }


        this.card = card;
        SetZoomMode(zoomMode);
        
        if (!isPreview)
        {
            GameController.Instance.AddCard(this);
        }

        if (card is PokemonCard)
        {
            var myRect = GetComponent<RectTransform>();
            int sortOrder = GetComponent<Canvas>().sortingOrder;
            float offsetSize = myRect.sizeDelta.x / 5;
            float attachedOffset = offsetSize;

            var pokemon = (PokemonCard)card;

            if (spawnAttachedEnergy)
            {
                SpawnAttachedEnergy(pokemon, zoomMode, myRect, sortOrder, offsetSize, attachedOffset);
            }

            SpawnAttachments(pokemon.AttachedTools, zoomMode);

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

    private void SpawnAttachedEnergy(PokemonCard card, ZoomMode zoomMode, RectTransform myRect, int sortOrder, float offsetSize, float attachedOffset)
    {
        if (card.AttachedEnergy == null)
        {
            return;
        }

        foreach (var attachedEnergy in card.AttachedEnergy)
        {
            var attachedObject = Instantiate(AttachedEnergyPrefab, AttachedEnergyList.transform);
            attachedObject.GetComponent<AttachedEnergy>().energyCard = attachedEnergy;
            attachedObject.GetComponent<Image>().sprite = EnergyResourceManager.Instance.GetSpriteForEnergyCard(attachedEnergy);
        }
    }

    private void SpawnAttachments(List<TrainerCard> attachments, ZoomMode zoomMode)
    {
        int extraX = 75;

        foreach (var attachment in attachments)
        {
            var gameObject = Instantiate(AttachmentPrefab, AttachmentParent);

            var rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(extraX / 2, 0);

            var zoomer = gameObject.GetComponent<CardZoomer>();
            zoomer.zoomMode = zoomMode;
            zoomer.extraX = extraX;
            extraX += 75;

            CardImageLoader.Instance.LoadSprite(attachment, gameObject.GetComponent<Image>());
        }
    }

    internal void insertAttachedEnergy(EnergyCard attachedEnergy)
    {
        var attachedObject = Instantiate(AttachedEnergyPrefab, AttachedEnergyList.transform);
        attachedObject.GetComponent<AttachedEnergy>().energyCard = attachedEnergy;
        attachedObject.transform.SetAsFirstSibling();
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
        damageTakenText.text = damage.ToString();
        damageTakenText.fontSize = 40;
        damageTakenText.gameObject.SetActive(true);

        while (damageTakenText.fontSize < 60)
        {
            damageTakenText.fontSize += 1.25f;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(0.3f);
        damageTakenText.gameObject.SetActive(false);
        damageTakenText.fontSize = 30;

        var actualDamageText = DamageDisplay.GetComponentInChildren<TextMeshProUGUI>();
        int currentDamage = int.Parse(actualDamageText.text.Trim());

        actualDamageText.text = (currentDamage + damage).ToString();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    Instantiate(HealEffect, EffectParent);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    Instantiate(Abilityeffect, EffectParent);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    Instantiate(DeathEffect, EffectParent);
        //}
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    var numbers = new[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
        //    StartOnDamageTaken(numbers[Random.Range(0, numbers.Length)]);
        //}
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
        isActivePokemon = true;
    }

    internal void SetIsBenched()
    {
        isActivePokemon = false;
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
