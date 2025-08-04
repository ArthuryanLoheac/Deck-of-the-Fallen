using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Transform parentBeforeDrag;
    public int positionBeforeDrag;
    public Image[] imageToDisableWhileDragging;
    public bool Zoomed = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Start drag the card
        CardsManager.instance.CardGrabbed = gameObject;
        UnitDrag.instance.IsDraggingCard = true;
        OnCardDropPLay.instance.startDrag();

        //Save datas of before drag
        parentBeforeDrag = transform.parent;
        positionBeforeDrag = transform.GetSiblingIndex();

        //Set In front
        transform.SetParent(transform.parent.parent);

        //Desactive detection by mouse to deteect object behind
        foreach (Image image in imageToDisableWhileDragging)
        {
            image.raycastTarget = false;
        }
        GetComponent<Card>().getSetCardStats(GetComponent<Card>().cardStats.artType).SetActiveZoomDrag(true, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Update pos when dragging
        transform.position = Input.mousePosition;
        GetComponent<Card>().getSetCardStats(GetComponent<Card>().cardStats.artType).SetActiveZoomDrag(true, false);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //When release card
        UnitDrag.instance.IsDraggingCard = false;
        OnCardDropPLay.instance.endDrag();

        ComputeNewPosition();
        CardsManager.instance.CardGrabbed = null;

        //Reactive raycast to detect click
        foreach(Image image in imageToDisableWhileDragging) {
            image.raycastTarget = true;
        }
        GetComponent<Card>().getSetCardStats(GetComponent<Card>().cardStats.artType).SetActiveZoomDrag(false, false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //When click normal spawn the ghost
        ZoomCardManager.instance.ActiveCard(GetComponent<Card>());
    }

    public void ResetPosition()
    {
        //Reset position at before drag state
        transform.SetParent(parentBeforeDrag);
        transform.SetSiblingIndex(positionBeforeDrag);
        CardsManager.instance.UpdatePosCards();
    }

    public int GetCardGarbPosition()
    {
        //Get Position of the card in the hand when drag
        Transform temp = transform.parent;
        transform.SetParent(parentBeforeDrag);
        int position = positionBeforeDrag;
        //Minimum
        if (parentBeforeDrag.childCount > 0) {
            if (GetComponent<RectTransform>().anchoredPosition.x <= parentBeforeDrag.GetChild(CardsManager.instance.cursorCards).GetComponent<RectTransform>().anchoredPosition.x)
                position = CardsManager.instance.cursorCards;
        }
        //Maximum
        if (parentBeforeDrag.childCount > 1) {
            if (GetComponent<RectTransform>().anchoredPosition.x >= parentBeforeDrag.GetChild(CardsManager.instance.cursorCards + Mathf.Min(CardsManager.instance.numberCardsShow, CardsManager.instance.numberCards) - 2).GetComponent<RectTransform>().anchoredPosition.x)
                position = CardsManager.instance.cursorCards + Mathf.Min(CardsManager.instance.numberCardsShow, CardsManager.instance.numberCards) - 1;
        }
        for (int i = 0; i < parentBeforeDrag.childCount - 2; i++) {
            if (parentBeforeDrag.GetChild(i).gameObject.activeInHierarchy && parentBeforeDrag.GetChild(i).GetComponent<RectTransform>().anchoredPosition.x <= GetComponent<RectTransform>().anchoredPosition.x
                && parentBeforeDrag.GetChild(i + 1).GetComponent<RectTransform>().anchoredPosition.x >= GetComponent<RectTransform>().anchoredPosition.x) {
                position = i + 1;
            }
        }
        transform.SetParent(temp);
        return position;
    }

    private void ComputeNewPosition()
    {
        //Set the new position
        if (transform.parent != parentBeforeDrag){
            transform.SetParent(parentBeforeDrag);
            transform.SetSiblingIndex(GetCardGarbPosition());
        }
        CardsManager.instance.UpdatePosCards();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.instance.PlaySound("CardSound");
        Zoomed = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Zoomed = false;
    }
}