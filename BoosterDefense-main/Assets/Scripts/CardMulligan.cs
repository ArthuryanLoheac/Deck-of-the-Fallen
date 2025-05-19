using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardMulligan : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int id;
    bool notDiscard;
    bool Destroyable;
    bool zoomed;
    Vector3 originalScale;

    public void Awake()
    {
        Vector3 a = GetComponent<RectTransform>().eulerAngles;
        a.y = 90;
        GetComponent<RectTransform>().eulerAngles = a;
    }

    public void Start()
    {
        ActiveDestroy(false);
        notDiscard = true;
        originalScale = GetComponent<RectTransform>().localScale;
        GetComponent<Animation>().Play("CardFlipSpawn");
    }

    public void DestroyCard()
    {
    }

    public void ActiveDestroy(bool active)
    {
        Destroyable = active;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Destroyable)
        {
            notDiscard = !notDiscard;
            MulliganManager.instance.RemoveCard(id);
            if (notDiscard)
                transform.SetSiblingIndex(id);
            else
                transform.SetAsFirstSibling();
        }
    }

    void Update()
    {
        float Scale = notDiscard ? (zoomed ? 1.1f : 1) : 0.85f;
        GetComponent<RectTransform>().localScale = originalScale * Scale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        zoomed = true;
        if (notDiscard)
            transform.SetAsLastSibling();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        zoomed = false;
        if (Destroyable)
        {
            if (notDiscard)
            {
                transform.SetSiblingIndex(id + MulliganManager.instance.cardsDiscarded.Count);
            }
        }
        else
            transform.SetSiblingIndex(id);
    }
}
