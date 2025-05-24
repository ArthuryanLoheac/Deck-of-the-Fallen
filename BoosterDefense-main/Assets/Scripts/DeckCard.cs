using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeckCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    GameObject cardDragged;
    float deckSize = 0.6f; // percentage of the screen (from the bottom)

    public void OnBeginDrag(PointerEventData eventData)
    {
        cardDragged = DeckMenuManager.instance.TakeCardFrom(GetComponent<Card>());
        cardDragged.transform.SetParent(DeckMenuManager.instance.canvasCardMoved.transform);
        SoundManager.instance.PlaySound("DrawCard");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (cardDragged)
            cardDragged.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (cardDragged)
        {
            if (eventData.position.y > Screen.height * deckSize)
            {
                DeckMenuManager.instance.AddCardTo(false, cardDragged);
            }
            else
            {
                DeckMenuManager.instance.AddCardTo(true, cardDragged);
            }
            cardDragged = null;
            SoundManager.instance.PlaySound("DrawCard");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DeckMenuManager.instance.SetStatsZommed(gameObject.GetComponent<Card>());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        SoundManager.instance.PlaySound("CardSound");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}