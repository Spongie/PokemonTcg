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
    public GameObject DamageDisplay;

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

    void Awake()
    {
        SetCard(new PokemonCard() { SetCode = "base1", IsRevealed = true, ImageUrl = "https://images.pokemontcg.io/base1/33_hires.png" }, ZoomMode.Center, true);
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

    public void SetCard(Card card, ZoomMode zoomMode, bool spawnAttachedEnergy)
    {
        if (card is PokemonCard)
        {
            pokemon = (PokemonCard)card;
        }

        this.card = card;
        SetZoomMode(zoomMode);
        
        if (card is PokemonCard)
        {
            var myRect = GetComponent<RectTransform>();
            int sortOrder = GetComponent<Canvas>().sortingOrder;
            float offsetSize = myRect.sizeDelta.x / 5;
            float attachedOffset = offsetSize;

            if (spawnAttachedEnergy)
            {
                SpawnAttachedEnergy(card, zoomMode, myRect, sortOrder, offsetSize, attachedOffset);
            }

            var pokemon = (PokemonCard)card;

            if (pokemon.DamageCounters > 0)
            {
                DamageDisplay.SetActive(true);
                DamageDisplay.GetComponentInChildren<TextMeshProUGUI>().text = pokemon.DamageCounters.ToString();
            }
            else
            {
                DamageDisplay.SetActive(false);
            }

            PoisonIcon.SetActive(pokemon.IsPoisoned);
            AsleepIcon.SetActive(pokemon.IsAsleep);
            ParalyzedIcon.SetActive(pokemon.IsParalyzed);
            ConfusedIcon.SetActive(pokemon.IsConfused);
            BurnedIcon.SetActive(pokemon.IsBurned);
        }

        StartCoroutine(LoadSprite(card));
    }

    private void SpawnAttachedEnergy(Card card, ZoomMode zoomMode, RectTransform myRect, int sortOrder, float offsetSize, float attachedOffset)
    {
        return;
        var activePokemonCard = GameController.Instance.Player.ActivePokemonCard;
        var otherActtive = GameController.Instance.OpponentPlayer.ActivePokemonCard;

        foreach (var attachedEnergy in ((PokemonCard)card).AttachedEnergy)
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
        attachedObject.transform.SetAsFirstSibling();
        attachedObject.GetComponent<Image>().sprite = EnergyResourceManager.Instance.GetSpriteForEnergyCard(attachedEnergy);
    }

    IEnumerator LoadSprite(Card card)
    {
        if (!card.IsRevealed)
        {
            art.sprite = GameController.Instance.CardBack;
        }
        else
        {
            string fullCardPath = Path.Combine(Application.streamingAssetsPath, "Cards", card.SetCode, card.GetImageName());
            string finalPath = "file:///" + fullCardPath;

            if (SpriteCache.Instance.cache.ContainsKey(fullCardPath))
            {
                art.sprite = SpriteCache.Instance.cache[fullCardPath];
            }
            else
            {
                using (var request = UnityWebRequestTexture.GetTexture(finalPath))
                {
                    yield return request.SendWebRequest();

                    if (request.isNetworkError || request.isHttpError)
                    {
                        Debug.LogError("Error fetching texture");
                    }

                    var texture = DownloadHandlerTexture.GetContent(request);
                    var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    art.sprite = sprite;

                    if (!SpriteCache.Instance.cache.ContainsKey(fullCardPath))
                    {
                        SpriteCache.Instance.cache.Add(fullCardPath, sprite);
                    }
                }
            }
        }
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
                effectPrefab = FightingEffect;
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Instantiate(DeathEffect, EffectParent);
        }
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
        popupHandler.gameObject.SetActive(true);
        popupHandler.Init(card);
    }
}
