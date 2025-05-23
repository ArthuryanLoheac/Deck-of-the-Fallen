using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetCardStats : MonoBehaviour
{
    
    public Image[] lstToTransparenceWhenBlocked;

    [Header("Card")]
    public Image Image;
    public TMP_Text textObject;

    [Header("Price")]
    public Image iconPrice;
    public TMP_Text textPrice;
    public TMP_Text textPrice_Free;
    [Header("Icons Price")]
    public Sprite scrapsIcon;
    public Sprite goldIcon;
    public Sprite foodIcon;



    [Header("Description")]
    public TMP_Text description;
    public TMP_Text story;


    [Header("Type")]
    public TMP_Text typeCard;
    [Header("HP")]
    public GameObject iconHP;
    public TMP_Text textHP;


    [Header("BG Contours")]
    public Image Contour;
    public Sprite ContoursCommon;
    public Image BG;

    private string GetSpriteType(TypeCard type)
    {
        switch (type)
        {
            case TypeCard.Sort:
                return "SPELL";
            case TypeCard.Npc:
                return "HEROS";
            case TypeCard.Batiment:
                return "BUILDING";
            case TypeCard.Vehicule:
                return "VEHICLE";
            case TypeCard.Equipement:
                return "EQUIPMENT";
            default:
                return "BUILDING";
        }
    }
    private Sprite getIconRessourcesCard(RessourceType nameRessource)
    {
        if (nameRessource == RessourceType.scraps)
        {
            return scrapsIcon;
        }
        else if (nameRessource == RessourceType.goldInGame)
        {
            return goldIcon;
        }
        else if (nameRessource == RessourceType.food)
        {
            return foodIcon;
        }
        return null;
    }
    
    private Color GetColorTransparent(Color col, bool b)
    {
        if (b)
            return new Color(col.r, col.g, col.b, 1);
        else
            return new Color(col.r, col.g, col.b, .7f);
    }

    public void MakeTransparent(bool b)
    {
        foreach (Image img in lstToTransparenceWhenBlocked)
        {
            img.color = GetColorTransparent(img.color, b);
        }
    }

    public void SetStats(CardStats stats)
    {
        textObject.text = stats.name;
        Image.sprite = stats.image;
        Vector3 v = Image.GetComponent<RectTransform>().localPosition;
        v.y = stats.offsetTop;
        Image.GetComponent<RectTransform>().localPosition = v;
        description.enabled = true;
        description.richText = true;
        description.text = stats.description;
        description.fontSize = stats.sizeFont;
        story.enabled = true;
        story.richText = true;
        story.text = stats.story;
        typeCard.text = GetSpriteType(stats.type);
        iconHP.SetActive(stats.hasHp);
        if (stats.hasHp)
        {
            textHP.text = stats.ghostToSpawn.GetComponent<placementInGrid>().objToSpawn.GetComponent<Life>().hp.ToString();
        }

        if (stats.rarity == Rarity.Rare)
            Contour.sprite = ContoursCommon;
        if (stats.rarity == Rarity.Common)
            Contour.sprite = ContoursCommon;
        if (stats.rarity == Rarity.SuperRare)
            Contour.sprite = ContoursCommon;

        if (stats.price <= 0)
        {
            iconPrice.gameObject.SetActive(false);
            textPrice.gameObject.SetActive(false);
            textPrice_Free.gameObject.SetActive(true);
        }
        else
        {
            iconPrice.gameObject.SetActive(true);
            textPrice.gameObject.SetActive(true);
            iconPrice.sprite = getIconRessourcesCard(stats.priceRessource);
            textPrice.text = stats.price.ToString();
            textPrice_Free.gameObject.SetActive(false);
        }
    }
}
