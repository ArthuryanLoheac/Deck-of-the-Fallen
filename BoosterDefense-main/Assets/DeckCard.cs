using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeckCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    GameObject cardDragged;

    public void OnBeginDrag(PointerEventData eventData)
    {
        cardDragged = DeckMenuManager.instance.TakeCardFrom(GetComponent<Card>());
        cardDragged.transform.SetParent(DeckMenuManager.instance.canvasCardMoved.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (cardDragged)
            cardDragged.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        double DeckSize = DeckMenuManager.instance.canvasDeckCards.GetComponent<RectTransform>().sizeDelta.y;

        if (cardDragged) {
            if (eventData.position.y > DeckSize) {
                DeckMenuManager.instance.AddCardTo(false, gameObject);
            } else {
                DeckMenuManager.instance.AddCardTo(true, gameObject);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DeckMenuManager.instance.SetStatsZommed(gameObject.GetComponent<Card>());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}