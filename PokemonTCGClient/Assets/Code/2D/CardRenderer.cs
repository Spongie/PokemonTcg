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

public class CardRenderer : MonoBehaviour, IPointerClickHandler
{
    private Dictionary<EnergyTypes, Sprite> icons;
    public Texture2D icon_atlas;
    public Text cardName;
    public Text hp;
    public Image art;
    public Image typeIcon;
    public Image weaknessIcon;
    public Image resistanceIcon;
    public GameObject retreatCost;
    public GameObject attacks;
    public GameObject attackPrefab;
    public GameObject abilityPrefab;
    public GameObject costPrefab;
    public Card card;
    public bool isActivePokemon;

    private void Awake()
    {
        icons = new Dictionary<EnergyTypes, Sprite>();

        foreach (var sprite in Resources.LoadAll<Sprite>(icon_atlas.name))
        {
            var type = (EnergyTypes)Enum.Parse(typeof(EnergyTypes), sprite.name);
            icons.Add(type, sprite);
        }
    }

    private void Start()
    {
        //SetCard(new Ekans(null));
    }

    public void SetCard(Card card)
    {
        this.card = card;

        if (card is PokemonCard)
        {
            SetPokemonCard((PokemonCard)card);
        }
    }

    public void SetPokemonCard(PokemonCard card)
    {
        cardName.text = card.PokemonName;
        hp.text = card.Hp.ToString();

        typeIcon.sprite = icons[card.PokemonType];

        GetComponentInChildren<TemplateSelector>().SetTemplate(card.PokemonType);

        if (card.Weakness != EnergyTypes.None)
        {
            weaknessIcon.sprite = icons[card.Weakness];
        }
        else
        {
            weaknessIcon.color = new Color(weaknessIcon.color.r, weaknessIcon.color.g, weaknessIcon.color.b, 0);
        }
        if (card.Resistance != EnergyTypes.None)
        {
            resistanceIcon.sprite = icons[card.Resistance];
        }
        else
        {
            resistanceIcon.color = new Color(resistanceIcon.color.r, resistanceIcon.color.g, resistanceIcon.color.b, 0);
        }

        for (int i = 0; i < card.RetreatCost; i++)
        {
            Instantiate(costPrefab, retreatCost.transform).GetComponent<Image>().sprite = icons[EnergyTypes.Colorless];
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
}
