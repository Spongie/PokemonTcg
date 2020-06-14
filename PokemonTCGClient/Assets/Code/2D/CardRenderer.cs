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
using BaseSet.PokemonCards;

public class CardRenderer : MonoBehaviour, IPointerClickHandler
{
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

    void Start()
    {
        //SetCard(new Bulbasaur(null) { IsRevealed = true }, ZoomMode.Center);
    }

    public void SetCard(Card card, ZoomMode zoomMode)
    {
        this.card = card;
        var zoomer = GetComponentInChildren<CardZoomer>();

        if (zoomer != null)
        {
            zoomer.zoomMode = zoomMode;
        }

        if (card is PokemonCard)
        {
            var myRect = GetComponent<RectTransform>();
            int sortOrder = GetComponent<Canvas>().sortingOrder;
            float offsetSize = myRect.sizeDelta.x / 5;
            float attachedOffset = offsetSize;

            SpawnAttachedEnergy(card, zoomMode, myRect, sortOrder, offsetSize, attachedOffset);

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
        var activePokemonCard = GameController.Instance.Player.ActivePokemonCard;

        if (activePokemonCard != null && activePokemonCard.Id.Equals(card.Id))
        {
            foreach (var attachedEnergy in ((PokemonCard)card).AttachedEnergy)
            {
                var attachedObject = Instantiate(AttachedCardPrefab, transform);
                var rect = attachedObject.GetComponent<RectTransform>();
                rect.sizeDelta = myRect.sizeDelta;
                rect.anchoredPosition = new Vector2(attachedOffset, rect.anchoredPosition.y);
                attachedOffset += offsetSize;

                attachedObject.GetComponent<Canvas>().sortingOrder = sortOrder;
                attachedObject.GetComponent<CardRenderer>().SetCard(attachedEnergy, zoomMode);
            }
        }
        else
        {
            foreach (var attachedEnergy in ((PokemonCard)card).AttachedEnergy)
            {
                var attachedObject = Instantiate(AttachedEnergyPrefab, AttachedEnergyList.transform);
                attachedObject.GetComponent<Image>().sprite = GetSpriteFromEnergyType(attachedEnergy.EnergyType);
            }
        }
    }

    private Sprite GetSpriteFromEnergyType(EnergyTypes energyType)
    {
        switch (energyType)
        {
            case EnergyTypes.Colorless:
                return DoubleColorlessIcon;
            case EnergyTypes.Water:
                return WaterIcon;
            case EnergyTypes.Fire:
                return FireIcon;
            case EnergyTypes.Grass:
                return GrassIcon;
            case EnergyTypes.Electric:
                return ElectricIcon;
            case EnergyTypes.Psychic:
                return PsychicIcon;
            case EnergyTypes.Fighting:
                return FightingIcon;
            case EnergyTypes.Darkness:
                return null;
            case EnergyTypes.Steel:
                return null;
            case EnergyTypes.Fairy:
                return null;
            case EnergyTypes.Dragon:
                return null;
            default:
                return SpecialIcon;
        }
    }

    IEnumerator LoadSprite(Card card)
    {
        if (!card.IsRevealed)
        {
            art.sprite = GameController.Instance.CardBack;
        }
        else
        {
            string fullCardPath = Path.Combine(Application.streamingAssetsPath, card.GetLogicalName()) + ".png";
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
            GameController.Instance.SpecialState == SpecialGameState.SelectEnergyToRetreat)
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
