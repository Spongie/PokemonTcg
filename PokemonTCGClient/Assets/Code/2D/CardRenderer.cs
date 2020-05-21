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
using TeamRocket.PokemonCards;

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
            int sortOrder = GetComponent<Canvas>().sortingOrder;
            float offsetSize = GetComponent<RectTransform>().sizeDelta.x / 5;
            float attachedOffset = offsetSize;
            foreach (var attachedCard in ((PokemonCard)card).AttachedEnergy)
            {
                var attachedObject = Instantiate(AttachedCardPrefab, transform);
                var rect = attachedObject.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(attachedOffset, rect.anchoredPosition.y);
                attachedOffset += offsetSize;

                attachedObject.GetComponent<Canvas>().sortingOrder = sortOrder;
                attachedObject.GetComponent<CardRenderer>().SetCard(attachedCard, zoomMode);
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
        }

        StartCoroutine(LoadSprite(card));
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

            using (var request = UnityWebRequestTexture.GetTexture(finalPath))
            {
                yield return request.SendWebRequest();

                if (request.isNetworkError || request.isHttpError)
                {
                    Debug.LogError("Error fetching texture");
                }

                var texture = DownloadHandlerTexture.GetContent(request);
                art.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
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

        if (GameController.Instance.SpecialState == SpecialGameState.SelectingColor)
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
