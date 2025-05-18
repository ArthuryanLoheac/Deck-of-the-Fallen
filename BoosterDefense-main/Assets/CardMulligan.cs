using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class CardMulligan : MonoBehaviour, IPointerClickHandler
{
    public int id;
    bool discard;
    bool Destroyable;
    Vector3 originalScale;

    public void Start()
    {
        ActiveDestroy(false);
        discard = true;
        originalScale = GetComponent<RectTransform>().localScale;
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
            discard = !discard;
            MulliganManager.instance.RemoveCard(id);
        }
    }

    void Update()
    {
        float Scale = discard ? 1 : 0.8f;
        GetComponent<RectTransform>().localScale = originalScale * Scale;
    }
}
