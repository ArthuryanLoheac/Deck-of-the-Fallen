using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Card : MonoBehaviour
{
    public Image iconObject;
    public TMP_Text textObject;
    [Header("Price")]
    public Image iconPrice;
    public TMP_Text textPrice;
    private Button button;
    [Header("Count")]
    [HideInInspector]public int cardCount;
    public TMP_Text textCardCount;
    [HideInInspector]public CardStats cardStats;
    public Image BG;
    public Image Fondu;
    [Header("Description")]
    public TMP_Text description;
    [Header("Icones Type")]
    public Image iconType;
    public GameObject iconHP;
    public TMP_Text textHP;
    [Header("Icones Images")]
    public Sprite Sort;
    public Sprite Npc;
    public Sprite Batiment;
    public Sprite Vehicule;
    public Sprite Equipement;

    private Sprite GetSpriteType(TypeCard type)
    {
        switch(type) {
            case TypeCard.Sort:
                return Sort;
            case TypeCard.Npc:
                return Npc;
            case TypeCard.Batiment:
                return Batiment;
            case TypeCard.Vehicule:
                return Vehicule;
            case TypeCard.Equipement:
                return Equipement;
            default :
                return Batiment;
        }
    }

    public void SetStats(CardStats stats, bool desctipionActive = false)
    {
        cardStats = stats;
        textObject.text = cardStats.name;
        iconObject.sprite = cardStats.image;
        cardCount = 1;
        description.enabled = true;
        description.richText = true;    
        description.text = cardStats.description;
        iconType.sprite = GetSpriteType(stats.type);
        iconHP.SetActive(stats.hasHp);
        if (stats.hasHp){
            textHP.text = stats.ghostToSpawn.GetComponent<placementInGrid>().objToSpawn.GetComponent<Life>().hp.ToString();
        }
    
        if (cardStats.price <= 0) {
            iconPrice.gameObject.SetActive(false);
            textPrice.gameObject.SetActive(false);
        } else {
            iconPrice.gameObject.SetActive(true);
            textPrice.gameObject.SetActive(true);
            iconPrice.sprite = RessourceManager.instance.GetRessourceIcon(cardStats.priceRessource);
            textPrice.text = cardStats.price.ToString();
        }
    }

    private Color GetColorTransparent(Color col, bool b)
    {
        if (b)
            return new Color(col.r, col.g, col.b, 1);
        else
            return new Color(col.r, col.g, col.b, .9f);
    }

    private void MakeTransparent(bool b)
    {
        iconObject.color = GetColorTransparent(iconObject.color, b);
        textObject.color = GetColorTransparent(textObject.color, b);
        iconPrice.color = GetColorTransparent(iconPrice.color, b);
        textPrice.color = GetColorTransparent(textPrice.color, b);
        textCardCount.color = GetColorTransparent(textCardCount.color, b);
        BG.color = GetColorTransparent(BG.color, b);
        Fondu.color = GetColorTransparent(Fondu.color, b);
    }

    void Start()
    {
        button = GetComponent<Button>();
    }

    private void UpdatePrice()
    {
        if (cardStats.price > 0 && button){
            if (RessourceManager.instance.GetRessourceAmount(cardStats.priceRessource) < cardStats.price)
                button.interactable = false;
            else
                button.interactable = true;
            MakeTransparent(button.interactable);
        }
    }

    private void UpdateCount()
    {
        if (cardCount > 1) {
            textCardCount.enabled = true;
            textCardCount.text = "x" + cardCount.ToString();
        } else {
            textCardCount.enabled = false;
        }
    }

    private void UpdateCard()
    {
        UpdatePrice();
        UpdateCount();
        //Destroy if no more card
        if (cardCount <= 0) {
            CardsManager.instance.RemovePackCard(gameObject);
        }
    }

    void Update()
    {
        if (cardStats != null) {
            UpdateCard();
        }
    }

    public void SellCard()
    {
        //Vend cette carte
        if (cardStats.CanBeSold) {
            RessourceManager.instance.AddRessource(cardStats.SoldType, cardStats.ValueSold);
            CardsManager.instance.RemoveCard(gameObject);
            CardsManager.instance.UpdatePosCards();
        }
    }
}
