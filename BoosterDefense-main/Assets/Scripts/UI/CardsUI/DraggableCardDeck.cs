using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableCardDeck : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public bool dragging;
    private Transform parentBeforeDrag;
    
    public Image[] imageToDisableWhileDragging;
    public DeckEmplacement emplacement;
    public void OnBeginDrag(PointerEventData eventData)
    {
        dragging = true;
        parentBeforeDrag = transform.parent;
        transform.SetParent(transform.parent.parent.parent);
        transform.SetSiblingIndex(transform.GetSiblingIndex());

        foreach(Image image in imageToDisableWhileDragging) {
            image.raycastTarget = false;
        }
        DropUIManagement.instance.ActiveDrop(emplacement);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;

        //reset pos
        transform.SetParent(parentBeforeDrag);
        GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

        //Reactive raycast to detect click
        foreach(Image image in imageToDisableWhileDragging) {
            image.raycastTarget = true;
        }
        DropUIManagement.instance.DesactiveDrop();
    }
}
