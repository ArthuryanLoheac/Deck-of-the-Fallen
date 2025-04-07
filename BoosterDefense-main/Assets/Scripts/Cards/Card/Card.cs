using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Card : MonoBehaviour
{
    
    public Image[] lstToTransparenceWhenBlocked;
    public Image iconObject;
    public TMP_Text textObject;
    [Header("Price")]
    public Image iconPrice;
    public TMP_Text textPrice;
    public TMP_Text textPrice_Free;
    private Button button;
    public Sprite scrapsIcon;
    public Sprite goldIcon;
    public Sprite foodIcon;
    private Vector3 originalPosTxtPrice;


    [Header("Count")]
    [HideInInspector]public int cardCount;
    public TMP_Text textCardCount;
    [HideInInspector]public CardStats cardStats;
    public Image BG;


    [Header("Description")]
    public TMP_Text description;


    [Header("Icones Type")]
    public TMP_Text iconType;
    public GameObject iconHP;
    public TMP_Text textHP;


    [Header("BG Contours")]
    public Image Contour;
    public Sprite ContoursCommon;
    public Sprite ContoursRare;
    public Sprite ContoursSuperRare;

    private string GetSpriteType(TypeCard type)
    {
        switch(type) {
            case TypeCard.Sort:
                return "SORT";
            case TypeCard.Npc:
                return "HEROS";
            case TypeCard.Batiment:
                return "BUILDING";
            case TypeCard.Vehicule:
                return "VEHICLE";
            case TypeCard.Equipement:
                return "EQUIPMENT";
            default :
                return "BUILDING";
        }
    }

    private Sprite getIconRessourcesCard(RessourceType nameRessource)
    {
        if (nameRessource == RessourceType.scraps) {
            return scrapsIcon;
        } else if (nameRessource == RessourceType.gold) {
            return goldIcon;
        } else if (nameRessource == RessourceType.food) {
            return foodIcon;
        }
        return null;
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
        iconType.text = GetSpriteType(stats.type);
        iconHP.SetActive(stats.hasHp);
        if (stats.hasHp){
            textHP.text = stats.ghostToSpawn.GetComponent<placementInGrid>().objToSpawn.GetComponent<Life>().hp.ToString();
        }
    
        if (stats.rarity == Rarity.Rare)
            Contour.sprite = ContoursRare;
        if (stats.rarity == Rarity.Common)
            Contour.sprite = ContoursCommon;
        if (stats.rarity == Rarity.SuperRare)
            Contour.sprite = ContoursSuperRare;
    
        if (cardStats.price <= 0) {
            iconPrice.gameObject.SetActive(false);
            textPrice.gameObject.SetActive(false);
            textPrice_Free.gameObject.SetActive(true);
        } else {
            iconPrice.gameObject.SetActive(true);
            textPrice.gameObject.SetActive(true);
            iconPrice.sprite = getIconRessourcesCard(cardStats.priceRessource);
            textPrice.text = cardStats.price.ToString();
            textPrice_Free.gameObject.SetActive(false);
            //if (cardStats.priceRessource == RessourceType.scraps) {
            //    textPrice.transform.position = originalPosTxtPrice + (Vector3.up) * 10f;
            //} else {
            //    textPrice.transform.position = originalPosTxtPrice;
            //}
        }
    }

    private Color GetColorTransparent(Color col, bool b)
    {
        if (b)
            return new Color(col.r, col.g, col.b, 1);
        else
            return new Color(col.r, col.g, col.b, .5f);
    }

    private void MakeTransparent(bool b)
    {
        foreach (Image img in lstToTransparenceWhenBlocked) {
            img.color = GetColorTransparent(img.color, b);
        }
    }

    void Start()
    {
        button = GetComponent<Button>();
        originalPosTxtPrice = textPrice.transform.position;
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
