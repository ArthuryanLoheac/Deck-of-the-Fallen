using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardMulligan : MonoBehaviour, IPointerClickHandler
{
    public int id;
    bool notDiscard;
    bool Destroyable;
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
        }
    }

    void Update()
    {
        float Scale = notDiscard ? 1 : 0.9f;
        GetComponent<RectTransform>().localScale = originalScale * Scale;
    }
}
