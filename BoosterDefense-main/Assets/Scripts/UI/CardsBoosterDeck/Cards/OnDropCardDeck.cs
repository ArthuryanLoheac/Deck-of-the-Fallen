using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnDropCardDeck : MonoBehaviour, IDropHandler
{
    public DeckEmplacement emplacement;

    private void AddCardToDeck(CardStats stats)
    {
        if (emplacement == DeckEmplacement.All) {
            DeckCardsManager.instance.AllCards.Add(stats);
        } else if (emplacement == DeckEmplacement.Deck) {
            DeckCardsManager.instance.deck.Add(stats);
        }
    }

    private void RemoveCardToDeck(CardStats stats, DeckEmplacement emplacementCard)
    {
        if (emplacementCard == DeckEmplacement.All) {
            DeckCardsManager.instance.AllCards.Remove(stats);
        } else if (emplacementCard == DeckEmplacement.Deck) {
            DeckCardsManager.instance.deck.Remove(stats);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        //When releasing a card on the sell part
        GameObject dropped = eventData.pointerDrag;
        CardStats stats = dropped.GetComponent<Card>().cardStats;
        DeckEmplacement emplacementCard = dropped.GetComponent<DraggableCardDeck>().emplacement;
        
        AddCardToDeck(stats);
        RemoveCardToDeck(stats, emplacementCard);
    
        DeckCardsManager.instance.SortAll();
    }
}
