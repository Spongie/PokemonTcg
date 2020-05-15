using System.Collections;
using TCGCards;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using Assets.Code;
using System;
using Assets.Code.UI.Game;

public class CardRenderer : MonoBehaviour, IPointerClickHandler
{
    public Image art;
    public Card card;
    public bool isActivePokemon;
    public bool isSelected;
    public GameObject overlay;

    public void SetCard(Card card, ZoomMode zoomMode)
    {
        this.card = card;
        GetComponentInChildren<CardZoomer>().zoomMode = zoomMode;
        StartCoroutine(LoadSprite(card));
    }

    IEnumerator LoadSprite(Card card)
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

    internal void FadeOut()
    {
        art.color = new Color(art.color.r, art.color.g, art.color.b, 0.5f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameController.Instance.selectFromListPanel.activeSelf)
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
}
