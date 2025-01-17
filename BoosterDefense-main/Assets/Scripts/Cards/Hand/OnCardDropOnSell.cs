using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnCardDropOnSell : MonoBehaviour, IDropHandler
{
    public TMP_Text PriceValue;
    public Image PriceType;
    private GameObject cardGrabbed;
    public void OnDrop(PointerEventData eventData)
    {
        //When releasing a card on the sell part
        GameObject dropped = eventData.pointerDrag;
        CardsManager.instance.AddCardToCardUsed(cardGrabbed.GetComponent<Card>().cardStats);
        dropped.GetComponent<Card>().SellCard();
        if (dropped) {
            dropped.GetComponent<DraggableCard>().ResetPosition();
        }
    }

    public void SetValues()
    {
        cardGrabbed = CardsManager.instance.CardGrabbed;
        PriceValue.text = cardGrabbed.GetComponent<Card>().cardStats.ValueSold.ToString();
        PriceType.sprite = RessourceManager.instance.GetRessourceIcon(cardGrabbed.GetComponent<Card>().cardStats.SoldType);
    }
}
