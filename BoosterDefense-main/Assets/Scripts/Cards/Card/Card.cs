using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;

[Serializable]
public class CardVisual
{
    public TypeCardArt typeCardArt;
    public SetCardClass setCardStats;
}

public class Card : MonoBehaviour
{
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj))
            return true;
        if (obj == null || GetType() != obj.GetType())
            return false;
        Card other = (Card)obj;
        return cardStats == other.cardStats;
    }

    public override int GetHashCode()
    {
        return cardStats != null ? cardStats.GetHashCode() : 0;
    }
    private Button button;

    [Header("Count")]
    [HideInInspector] public int cardCount;
    public TMP_Text textCardCount;
    [HideInInspector] public CardStats cardStats;

    public CardVisual[] cardVisuals;

    SetCardClass getSetCardStats(TypeCardArt artType)
    {
        foreach (CardVisual style in cardVisuals)
        {
            if (style.typeCardArt == artType)
            {
                return style.setCardStats;
            }
        }
        return null;
    }
    void DisableNotSetCardStats(TypeCardArt artType)
    {
        foreach (CardVisual style in cardVisuals)
        {
            if (style.typeCardArt != artType)
            {
                style.setCardStats.gameObject.SetActive(false);
            }
            else    
            {
                style.setCardStats.gameObject.SetActive(true);
            }
        }
    }


    public void SetStats(CardStats stats)
    {
        cardStats = stats;
        cardCount = 1;
        getSetCardStats(stats.artType).SetStats(stats);
        DisableNotSetCardStats(stats.artType);
    }
    private void MakeTransparent(bool b)
    {
        getSetCardStats(cardStats.artType).MakeTransparent(b);
    }

    public static bool operator ==(Card a, Card b)
    {
        if (ReferenceEquals(a, b))
            return true;
        if (a is null || b is null)
            return false;
        return a.cardStats == b.cardStats;
    }

    public static bool operator !=(Card a, Card b)
    {
        return !(a == b);
    }

    void Start()
    {
        button = GetComponent<Button>();
    }

    private void UpdatePrice()
    {
        if (cardStats.price > 0 && button)
        {
            if (RessourceManager.instance.GetRessourceAmount(cardStats.priceRessource) < cardStats.price)
                button.interactable = false;
            else
                button.interactable = true;
            MakeTransparent(button.interactable);
        }
    }

    private void UpdateCount()
    {
        if (cardCount > 1)
        {
            textCardCount.enabled = true;
            textCardCount.text = "x" + cardCount.ToString();
        }
        else
        {
            textCardCount.enabled = false;
        }
    }

    private void UpdateCard()
    {
        UpdatePrice();
        UpdateCount();
        //Destroy if no more card
        if (cardCount <= 0)
        {
            CardsManager.instance.RemovePackCard(gameObject);
        }
    }

    void Update()
    {
        if (cardStats != null)
        {
            UpdateCard();
        }
    }

    public void SellCard()
    {
        //Vend cette carte
        if (cardStats.CanBeSold)
        {
            RessourceManager.instance.AddRessource(cardStats.SoldType, cardStats.ValueSold);
            CardsManager.instance.RemoveCard(gameObject);
            CardsManager.instance.UpdatePosCards();
        }
    }
}
