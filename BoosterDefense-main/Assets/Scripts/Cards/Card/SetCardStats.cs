    using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetCardStats : SetCardClass, IPointerEnterHandler, IPointerExitHandler
{
    
    public Image[] lstToTransparenceWhenBlocked;

    [Header("Card")]
    public Image Image;
    public Image BackGround;
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

    bool isZoomed;
    CardStats myStats;
    Vector3 scaleOriginal;
    Vector3 scaleBGOriginal;

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
    
    
    public override void MakeTransparent(bool b) 
    {
        foreach (Image img in lstToTransparenceWhenBlocked)
        {
            img.color = GetColorTransparent(img.color, b);
        }
    }

    private void SetRectTransform(Image img, float offset)
    {
        Vector3 v = img.GetComponent<RectTransform>().localPosition;
        v.y = offset;
        img.GetComponent<RectTransform>().localPosition = v;
    }

    public override void SetStats(CardStats stats)
    {
        myStats = stats;
        textObject.text = stats.name;
        Image.sprite = stats.image;
        BackGround.sprite = stats.backGround;
        SetRectTransform(Image, stats.offsetTop);
        SetRectTransform(BackGround, stats.offsetTop);
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
            textHP.text = stats.ghostToSpawn.GetComponent<placementInGrid>().objToSpawn.GetComponent<Life>().hp.ToString();

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
        scaleOriginal = Image.GetComponent<RectTransform>().localScale;
        scaleBGOriginal = BackGround.GetComponent<RectTransform>().localScale;
    }

    void Update()
    {
        float offset = isZoomed ? myStats.offsetFinal : myStats.offsetTop;
        float scale = isZoomed ? 1.1f : 1;

        SetRectTransform(Image, offset);
        Image.GetComponent<RectTransform>().localScale = new Vector3(
            scaleOriginal.y * scale, scaleOriginal.x * scale, scaleOriginal.z * scale
        );
        if (myStats.BgFollow)
        {
            SetRectTransform(BackGround, offset);
            BackGround.GetComponent<RectTransform>().localScale = new Vector3(
                scaleBGOriginal.x * scale, scaleBGOriginal.y * scale, scaleBGOriginal.z * scale
        );
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isZoomed = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isZoomed = false;
    }
}
