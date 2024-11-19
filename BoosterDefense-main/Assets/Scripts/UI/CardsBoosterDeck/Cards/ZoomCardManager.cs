using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCardManager : MonoBehaviour
{
    public static ZoomCardManager instance;
    public GameObject canvasZoom;
    public Card cardInstance;
    private CardStats cardStats;

    private bool CardZoomActive;

    void Awake()
    {
        instance = this;
        CardZoomActive = false;
    }

    public void ActiveCardZoom()
    {
        CardZoomActive = true;
        cardInstance.SetStats(cardStats, true);
    }
    public void DesactiveCardZoom()
    {
        CardZoomActive = false;
        cardStats = null;
    }

    public void ActiveCard(Card card)
    {
        if (cardStats != card.cardStats){
            cardStats = card.cardStats;
            ActiveCardZoom();
        } else {
            cardStats = null;
            DesactiveCardZoom();
        }
    }

    void Update()
    {
        canvasZoom.SetActive(CardZoomActive);
    }
}
