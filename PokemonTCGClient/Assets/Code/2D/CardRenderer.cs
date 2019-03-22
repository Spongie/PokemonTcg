using System.Collections;
using System.Collections.Generic;
using TCGCards;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using Assets.Code;
using Assets.Code._2D;
using TMPro;

public class CardRenderer : MonoBehaviour, IPointerClickHandler
{
    public Text cardName;
    public Text hpNumber;
    public Text hpFixed;
    public Text trainerCardName;
    public TextMeshProUGUI trainerCardDescription;
    public Sprite TrainerCardTemplate;
    public Image art;
    public Image typeIcon;
    public Image colorImage;
    public Image weaknessIcon;
    public Image resistanceIcon;
    public Image template;
    public GameObject retreatCost;
    public GameObject attacks;
    public GameObject attackPrefab;
    public GameObject abilityPrefab;
    public GameObject costPrefab;
    public SelectIndicator selectIndicator;
    public Card card;
    public bool isActivePokemon;
    private EnergyResourceManager energyResources;

    private void Start()
    {
        //SetCard(new Ekans(null)); //TODO remove
        energyResources = FindObjectOfType<EnergyResourceManager>();
    }

    public void SetCard(Card card)
    {
        this.card = card;

        if (card is PokemonCard)
        {
            SetPokemonCard((PokemonCard)card);
        }
        else if (card is EnergyCard)
        {
            SetEnergyCard((EnergyCard)card);
        }
        else if (card is TrainerCard)
        {
            SetTrainerCard((TrainerCard)card);
        }
    }

    public void SetTrainerCard(TrainerCard card)
    {
        colorImage.enabled = false;
        cardName.enabled = false;
        hpNumber.enabled = false;
        hpFixed.enabled = false;
        typeIcon.enabled = false;
        weaknessIcon.enabled = false;
        resistanceIcon.enabled = false;
        retreatCost.SetActive(false);
        attacks.SetActive(false);
        trainerCardDescription.gameObject.SetActive(true);
        trainerCardName.gameObject.SetActive(true);

        trainerCardName.text = card.Name;
        trainerCardDescription.text = card.Description;
        template.sprite = TrainerCardTemplate;

        StartCoroutine(LoadSprite(card));
    }

    public void SetEnergyCard(EnergyCard card)
    {
        colorImage.enabled = false;
        cardName.enabled = false;
        hpNumber.enabled = false;
        hpFixed.enabled = false;
        template.enabled = false;
        typeIcon.enabled = false;
        weaknessIcon.enabled = false;
        resistanceIcon.enabled = false;
        retreatCost.SetActive(false);
        attacks.SetActive(false);
        StartCoroutine(LoadSprite(card));
    }

    public void SetPokemonCard(PokemonCard card)
    {
        cardName.text = card.PokemonName;
        hpNumber.text = card.Hp.ToString();

        typeIcon.sprite = energyResources.Icons[card.PokemonType];

        GetComponentInChildren<TemplateSelector>().SetTemplate(card.PokemonType);

        if (card.Weakness != EnergyTypes.None)
        {
            weaknessIcon.sprite = energyResources.Icons[card.Weakness];
        }
        else
        {
            weaknessIcon.color = new Color(weaknessIcon.color.r, weaknessIcon.color.g, weaknessIcon.color.b, 0);
        }
        if (card.Resistance != EnergyTypes.None)
        {
            resistanceIcon.sprite = energyResources.Icons[card.Resistance];
        }
        else
        {
            resistanceIcon.color = new Color(resistanceIcon.color.r, resistanceIcon.color.g, resistanceIcon.color.b, 0);
        }

        for (int i = 0; i < card.RetreatCost; i++)
        {
            Instantiate(costPrefab, retreatCost.transform).GetComponent<Image>().sprite = energyResources.Icons[EnergyTypes.Colorless];
        }

        if (card.Ability != null)
        {
            Instantiate(abilityPrefab, attacks.transform).GetComponent<AbilityRenderer>().SetAbility(card.Ability);
        }

        foreach (var attack in card.Attacks)
        {
            Instantiate(attackPrefab, attacks.transform).GetComponent<AttackRenderer>().SetAttack(attack);
        }

        StartCoroutine(LoadSprite(card));
    }

    internal void SetSelected(bool selected)
    {
        selectIndicator.SetSelected(selected);
    }

    IEnumerator LoadSprite(Card card)
    {
        string fullCardPath = Path.Combine(Application.streamingAssetsPath, card.GetLogicalName()) + ".jpg";
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

    public void OnPointerClick(PointerEventData eventData)
    {
        GameController.Instance.OnCardClicked(this);
    }

    internal void SetIsActivePokemon()
    {
        isActivePokemon = true;
        GetComponent<CardZoomer>().enabled = false;
        GetComponent<CardDragger>().enabled = false;
    }

    internal void SetIsBenched()
    {
        isActivePokemon = false;
        GetComponent<CardZoomer>().enabled = false;
        GetComponent<CardDragger>().enabled = false;
    }
}
